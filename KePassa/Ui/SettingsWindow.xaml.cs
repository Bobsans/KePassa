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
}
