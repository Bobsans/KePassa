using System.Collections.ObjectModel;

namespace SecretStore;

public class MainDataContext {
    public ObservableCollection<RecordGroup> Groups { get; set; } = [];
}
