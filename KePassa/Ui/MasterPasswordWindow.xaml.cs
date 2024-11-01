using System.Security.Cryptography;
using System.Text;
using System.Windows;
using SecretStore.Core;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class MasterPasswordWindow {
    private readonly MasterPasswordWindowModel _model;
    private readonly SettingManager _settingManager;

    public MasterPasswordWindow(
        MasterPasswordWindowModel model,
        SettingManager settingManager
    ) {
        DataContext = _model = model;
        _settingManager = settingManager;
        InitializeComponent();
    }

    private void ButtonOkOnClick(object sender, RoutedEventArgs e) {
        _settingManager.Current.MasterPasswordHash = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(_model.Password)));
        _settingManager.Save(_settingManager.Current);
        Close();
    }
}
