namespace SecretStore.Model;

public class SettingsStoragePageModel(
    SettingsWindowModel settingsWindowModel
) : BaseModel {
    public string StorageFileLocation {
        get => settingsWindowModel.Settings.StorageFileLocation;
        set => settingsWindowModel.Settings.StorageFileLocation = value;
    }
}
