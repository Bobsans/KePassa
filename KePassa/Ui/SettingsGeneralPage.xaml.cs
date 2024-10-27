using System.Windows.Controls;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class SettingsGeneralPage : Page {
    private SettingsGeneralPageViewModel ViewModel { get; }

    public SettingsGeneralPage(SettingsGeneralPageViewModel viewModel) {
        DataContext = ViewModel = viewModel;
        InitializeComponent();
    }
}

