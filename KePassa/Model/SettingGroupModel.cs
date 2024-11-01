using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace SecretStore.Model;

public class SettingGroupModel : BaseModel {
    public string Name { get; set; } = string.Empty;

    public required Action<NavigationService> Nvaigate { get; init; }

    public ObservableCollection<SettingGroupModel> Children { get; set; } = [];
}
