namespace SecretStore.Model;

public class SettingsGeneralPageModel(
    SettingsWindowModel settingsWindowModel
) : BaseModel {
    public string PasswordView => settingsWindowModel.Settings.MasterPasswordHash is not null ? "••••••••••••••" : string.Empty;
}
