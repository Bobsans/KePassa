using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace SecretStore.Ui.Model;

public class SettingGroupViewModel : BaseViewModel {
    public string Name { get; set; } = string.Empty;

    public Action<NavigationService> Nvaigate { get; init; }

    public ObservableCollection<SettingGroupViewModel> Children { get; set; } = [];
}
