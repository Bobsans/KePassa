using System.Security.Cryptography;
using System.Text;
using System.Windows;
using SecretStore.Core;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class MasterPasswordWindow {
    private readonly MasterPasswordWindowViewModel _model;
    private readonly SettingsManager _settingsManager;

    public MasterPasswordWindow(
        MasterPasswordWindowViewModel viewModel,
        SettingsManager settingsManager
    ) {
        DataContext = _model = viewModel;
        _settingsManager = settingsManager;
        InitializeComponent();
    }

    private void ButtonOkOnClick(object sender, RoutedEventArgs e) {
        _settingsManager.Current.MasterPasswordHash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(_model.Password)));
        _settingsManager.Save(_settingsManager.Current);
        Close();
    }
}
