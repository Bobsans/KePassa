using System.Security.Cryptography;
using System.Text;
using System.Windows;
using SecretStore.Core;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class MasterPasswordWindow {
    private readonly MasterPasswordWindowViewModel _model;
    private readonly SettingManager _settingManager;

    public MasterPasswordWindow(
        MasterPasswordWindowViewModel viewModel,
        SettingManager settingManager
    ) {
        DataContext = _model = viewModel;
        _settingManager = settingManager;
        InitializeComponent();
    }

    private void ButtonOkOnClick(object sender, RoutedEventArgs e) {
        _settingManager.Current.MasterPasswordHash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(_model.Password)));
        _settingManager.Save(_settingManager.Current);
        Close();
    }
}
