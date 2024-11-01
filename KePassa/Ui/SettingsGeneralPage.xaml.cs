using System.Windows.Controls;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class SettingsGeneralPage : Page {
    private SettingsGeneralPageModel Model { get; }

    public SettingsGeneralPage(SettingsGeneralPageModel model) {
        DataContext = Model = model;
        InitializeComponent();
    }
}

