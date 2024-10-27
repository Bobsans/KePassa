using System.Windows;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class SettingsWindow {
    private readonly SettingsWindowViewModel _model;

    public SettingsWindow(SettingsWindowViewModel viewModel) {
        DataContext = _model = viewModel;

        InitializeComponent();

        TreeViewSidebar.SelectedItemChanged += (_, _) => {
            if (TreeViewSidebar.SelectedValue is SettingGroupViewModel item) {
                item.Nvaigate(FrameContent.NavigationService);
            }
        };
    }

    private void ButtonCancelOnClick(object sender, RoutedEventArgs e) {
        Close();
    }

    private void ButtonSaveOnClick(object sender, RoutedEventArgs e) {
        _model.SettingsManager.Save(_model.Settings);
        _model.SettingsManager.Load();
        Close();
    }
}
