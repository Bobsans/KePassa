using System.Collections.ObjectModel;

namespace SecretStore.Ui.Model;

public class RecordViewModel {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public ObservableCollection<RecordViewModel> Children { get; set; } = [];
}
