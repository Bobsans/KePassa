using System.Collections.ObjectModel;
using DimTim.DependencyInjection;

namespace SecretStore.Ui.Model;

public class SettingsWindowViewModel(IScope scope) : BaseViewModel {
    public ObservableCollection<SettingGroupViewModel> Groups { get; set; } = [
        new() {
            Name = "General",
            Nvaigate = navigation => navigation.Navigate(scope.Resolve<SettingsGeneralPage>())
        },
        new() {
            Name = "Storage",
            Nvaigate = navigation => navigation.Navigate(scope.Resolve<SettingsStoragePage>())
        }
    ];

    private SettingGroupViewModel? _currentGroup;

    public SettingGroupViewModel? CurrentGroup {
        get => _currentGroup;
        set => SetField(ref _currentGroup, value);
    }
}
