using System.Collections.ObjectModel;
using SecretStore.Data;

namespace SecretStore.Ui.Model;

public class RecordViewModel {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ObservableCollection<RecordViewModel> Children { get; set; } = [];

    public static RecordViewModel From(Record record) => new() {
        Name = record.Name,
        Description = record.Description,
        Content = record.Content,
        Children = new ObservableCollection<RecordViewModel>(record.Children.Select(From))
    };

    public Record ToRecord() => new() {
        Name = Name,
        Description = Description,
        Content = Content,
        Children = Children.Select(it => it.ToRecord()).ToList()
    };

    public static explicit operator RecordViewModel(Record value) => From(value);
    public static implicit operator Record(RecordViewModel value) => value.ToRecord();
}
