using System.Windows;
using SecretStore.Core;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class SettingsWindow {
    private readonly SettingManager _settingManager;
    private readonly SettingsWindowViewModel _model;

    public SettingsWindow(SettingsWindowViewModel viewModel, SettingManager settingManager) {
        DataContext = _model = viewModel;
        _settingManager = settingManager;

        Owner = Application.Current.MainWindow;
        InitializeComponent();
    }

    private void ButtonCancelOnClick(object sender, RoutedEventArgs e) {
        Close();
    }

    private void ButtonSaveOnClick(object sender, RoutedEventArgs e) {
        _settingManager.Save(_model.Settings);
        _settingManager.Load();
        Close();
    }

    private void TreeViewSidebarOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        if (e.NewValue is SettingGroupViewModel item) {
            item.Nvaigate(FrameContent.NavigationService);
        }
    }
}
