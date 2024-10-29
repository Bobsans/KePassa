using System.Collections.ObjectModel;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Data;

namespace SecretStore.Ui.Model;

public class SettingsWindowViewModel(IScope scope, SettingManager settingManager) : BaseViewModel {
    public readonly Settings Settings = settingManager.Current.Clone();

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

    public SettingGroupViewModel? CurrentGroup { get; set; }
}
