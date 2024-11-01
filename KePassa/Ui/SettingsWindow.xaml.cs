using System.Windows;
using SecretStore.Core;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class SettingsWindow {
    private readonly SettingManager _settingManager;
    private readonly SettingsWindowModel _model;

    public SettingsWindow(SettingsWindowModel model, SettingManager settingManager) {
        DataContext = _model = model;
        _settingManager = settingManager;

        Owner = Application.Current.MainWindow;
        InitializeComponent();
    }

    private void ButtonCancelOnClick(object sender, RoutedEventArgs e) {
        Close();
    }

    private void ButtonSaveOnClick(object sender, RoutedEventArgs e) {
        _settingManager.Save(_model.Settings);
        Close();
    }

    private void TreeViewSidebarOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        if (e.NewValue is SettingGroupModel item) {
            item.Nvaigate(FrameContent.NavigationService);
        }
    }
}
