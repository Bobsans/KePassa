using SecretStore.Data;

namespace SecretStore.Ui.Model;

public class RecordGroupViewModel {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RecordViewModel> Records { get; set; } = [];

    public static RecordGroupViewModel From(RecordGroup group) => new() {
        Name = group.Name,
        Description = group.Description,
        Records = group.Records.Select(RecordViewModel.From).ToList()
    };

    public RecordGroup ToRecordGroup() => new RecordGroup {
        Name = Name,
        Description = Description,
        Records = Records.Select(it => it.ToRecord()).ToList()
    };

    public static explicit operator RecordGroupViewModel(RecordGroup group) => From(group);
    public static implicit operator RecordGroup(RecordGroupViewModel model) => model.ToRecordGroup();
}
