using System.IO;
using DimTim.Logging;
using KePassa.Core.Abstraction;
using KePassa.Core.Data;
using MessagePack;
using SecretStore.Data;

namespace SecretStore.Core;

public class RecordManager(
    Settings settings,
    ILogger logger
) {
    public event Action? OnReload;
    public event Action<IRecord, Guid?>? OnChanged;
    public event Action<IRecord, Guid?>? OnDeleted;

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
                var item = Find(id, category.Children);
                if (item is not null) {
                    return item;
                }
            }
        }

        return null;
    }

    public void Delete(Guid id) {
        _ = Records.Any(it => InnerDelete(id, it));
    }

    private bool InnerDelete(Guid id, IRecord current, RecordCategory? parent = null) {
        if (current.Id == id) {
            if (parent is not null) {
                parent.Children.Remove(current);
            } else {
                Records.Remove(current);
            }

            OnDeleted?.Invoke(current, parent?.Id);
            Save();

            return true;
        }

        if (current is RecordCategory { Children.Count: > 0 } category) {
            return category.Children.Any(it => InnerDelete(id, it, category));
        }

        return false;
    }
}
