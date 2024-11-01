using System.IO;
using DimTim.Logging;
using KePassa.Core.Abstraction;
using KePassa.Core.Data;
using KePassa.Core.Services;
using SecretStore.Data;

namespace SecretStore.Core;

public class RecordManager(
    Settings settings,
    Serializer serializer,
    ILogger logger
) {
    public event Action? OnReload;
    public event Action<IRecord, Guid?>? OnChanged;

    public List<IRecord> Records { get; private set; } = [];
    public bool IsStorageExists => Path.Exists(settings.StorageFileLocation);

    public void Load() {
        if (IsStorageExists) {
            var data = File.ReadAllBytes(settings.StorageFileLocation);
            Records = serializer.Deserialize<List<IRecord>>(Encryptor.Decrypt(data, settings.MasterPasswordHash!));
            logger.Info($"Loaded {Records.Count} records");
            OnReload?.Invoke();
        }
    }

    public void Save() {
        var encrypted = Encryptor.Encrypt(serializer.Serialize(Records), settings.MasterPasswordHash!);
        File.WriteAllBytes(settings.StorageFileLocation, encrypted);
        logger.Info("Records have been saved.");
    }

    public void AddOrUpdate<T>(Guid modelId, Guid? parentId, Action<T> action) where T : class, IRecord, new() {
        var record = Find(modelId, Records) as T ?? new T();
        action(record);
        if (parentId.HasValue) {
            var parent = Find(parentId.Value, Records);
            if (parent is RecordCategory category) {
                category.Children.Add(record);
            }
        } else {
            Records.Add(record);
        }
        OnChanged?.Invoke(record, parentId);
        Save();
    }

    private static IRecord? Find(Guid id, IEnumerable<IRecord> source) {
        foreach (var record in source) {
            if (record.Id == id) {
                return record;
            }

            if (record is RecordCategory { Children.Count: > 0 } category) {
                return Find(id, category.Children);
            }
        }

        return null;
    }
}
