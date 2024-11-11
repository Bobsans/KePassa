using System.IO;
using DimTim.Logging;
using KePassa.Core.Abstraction;
using MessagePack;
using SecretStore.Data;

namespace SecretStore.Core;

public class RecordManager(
    Settings settings,
    ILogger logger
) {
    public event Action? OnReload;
    public event Action<IRecord>? OnChanged;
    public event Action<IRecord>? OnDeleted;

    public List<IRecord> Records { get; private set; } = [];
    public bool IsStorageExists => Path.Exists(settings.StorageFileLocation);

    public void Load() {
        if (IsStorageExists) {
            var data = File.ReadAllBytes(settings.StorageFileLocation);
            Records = MessagePackSerializer.Deserialize<List<IRecord>>(Encryptor.Decrypt(data, settings.MasterPasswordHash!));
            logger.Info($"Loaded {Records.Count} records");
            OnReload?.Invoke();
        }
    }

    public void Save() {
        var encrypted = Encryptor.Encrypt(MessagePackSerializer.Serialize(Records), settings.MasterPasswordHash!);
        File.WriteAllBytes(settings.StorageFileLocation, encrypted);
        logger.Info("Records have been saved.");
    }

    private T Add<T>() where T : IRecord, new() {
        var record = new T();
        Records.Add(record);
        return record;
    }

    public void AddOrUpdate<T>(Guid id, Action<T> action) where T : class, IRecord, new() {
        var record = Records.Find(it => it.Id == id) as T ?? Add<T>();
        action(record);
        OnChanged?.Invoke(record);
        Save();
    }

    public void Delete(Guid id) {
        var record = Records.Find(it => it.Id == id);
        if (record is not null) {
            Records.Remove(record);
            OnDeleted?.Invoke(record);
        }
    }
}
