using KePassa.Core.Data;

namespace SecretStore.Model;

public class RecordModel : BaseModel, IRecordModel {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public void Update(Record record) {
        Id = record.Id;
        Name = record.Name;
        Description = record.Description;
        Content = record.Content;
    }

    public void Update(RecordModel record) {
        Id = record.Id;
        Name = record.Name;
        Description = record.Description;
        Content = record.Content;
    }

    public static RecordModel From(Record record) => new() {
        Id = record.Id,
        Name = record.Name,
        Description = record.Description,
        Content = record.Content
    };

    public override string ToString() {
        return $"RecordModel [{Name}]";
    }
}
