using System.Collections.ObjectModel;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Data;
using SecretStore.Ui;

namespace SecretStore.Model;

public class SettingsWindowModel(IScope scope, SettingManager settingManager) : BaseModel {
    public readonly Settings Settings = settingManager.Current.Clone();

    public ObservableCollection<SettingGroupModel> Groups { get; set; } = [
        new() {
            Name = "General",
            Nvaigate = navigation => navigation.Navigate(scope.Resolve<SettingsGeneralPage>())
        },
        new() {
            Name = "Storage",
            Nvaigate = navigation => navigation.Navigate(scope.Resolve<SettingsStoragePage>())
        }
    ];

    public SettingGroupModel? CurrentGroup { get; set; }
}
