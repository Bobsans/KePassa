using SecretStore.Data;

namespace SecretStore.Ui.Model;

public class RecordViewModel {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public static RecordViewModel From(Record record) => new() {
        Name = record.Name,
        Description = record.Description,
        Content = record.Content
    };

    public Record ToRecord() => new() {
        Name = Name,
        Description = Description,
        Content = Content
    };

    public static explicit operator RecordViewModel(Record value) => From(value);
    public static implicit operator Record(RecordViewModel value) => value.ToRecord();
}
