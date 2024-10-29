using System.Windows.Controls;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class SettingsStoragePage : Page {
    private SettingsStoragePageViewModel _model;

    public SettingsStoragePage(SettingsStoragePageViewModel viewModel) {
        DataContext = _model = viewModel;
        InitializeComponent();
    }
}

