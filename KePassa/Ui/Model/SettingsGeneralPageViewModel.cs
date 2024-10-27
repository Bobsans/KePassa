namespace SecretStore.Ui.Model;

public class SettingsGeneralPageViewModel(
    SettingsWindowViewModel settingsWindowViewModel
) : BaseViewModel {
    public string PasswordView => settingsWindowViewModel.Settings.MasterPasswordHash is not null ? "••••••••••••••" : string.Empty;
}
