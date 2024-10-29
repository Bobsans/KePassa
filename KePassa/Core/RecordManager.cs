using System.IO;
using DimTim.Logging;
using MessagePack;
using SecretStore.Data;
using SecretStore.Ui.Model;

namespace SecretStore.Core;

public class RecordManager(
    Settings settings,
    ILogger logger
) {
    private List<Record> _records = [];

    public List<RecordViewModel> Tree => BuildTree();

    public event Action? OnReload;
    public event Action<Record>? OnRecordChanged;
    public event Action<Record>? OnRecordAdded;

    public void Load() {
        var data = File.ReadAllBytes(settings.StorageFileLocation);
        var decrypted = Encryptor.Decrypt(data, settings.MasterPasswordHash!);
        _records = MessagePackSerializer.Deserialize<List<Record>>(decrypted);
        logger.Info($"Loaded {_records.Count} records");
        OnReload?.Invoke();
    }

    public void Save() {
        var buffer = MessagePackSerializer.Serialize(_records);
        var encrypted = Encryptor.Encrypt(buffer, settings.MasterPasswordHash!);
        File.WriteAllBytes(settings.StorageFileLocation, encrypted);
        logger.Info("Records have been saved.");
    }

    public Record? Get(Guid id) {
        return _records.FirstOrDefault(it => it.Id == id);
    }

    private List<RecordViewModel> BuildTree() {
        return _records.Where(it => it.ParentId is null).Select(RecordToViewModel).ToList();
    }

    private RecordViewModel RecordToViewModel(Record record) => new() {
        Id = record.Id,
        Name = record.Name,
        Description = record.Description,
        Content = record.Content,
        ParentId = record.ParentId,
        Children = _records.Where(it => it.ParentId == record.Id).Select(RecordToViewModel).ToList()
    };

    public void Save(RecordViewModel model) {
        var record = Get(model.Id);
        if (record is null) {
            Add(model);
        } else {
            Update(record, model);
        }
    }

    private void Add(RecordViewModel model) {
        var record = new Record {
            Name = model.Name,
            Description = model.Description,
            Content = model.Content,
            ParentId = model.ParentId
        };
        _records.Add(record);
        Save();
        OnRecordAdded?.Invoke(record);
    }

    private void Update(Record record, RecordViewModel model) {
        record.Name = model.Name;
        record.Description = model.Description;
        record.Content = model.Content;
        record.ParentId = model.ParentId;
        Save();
        OnRecordChanged?.Invoke(record);
    }
}
