namespace SecretStore.Ui.Model;

public class SettingsStoragePageViewModel(
    SettingsWindowViewModel settingsWindowViewModel
) : BaseViewModel {
    public string StoragePathView => settingsWindowViewModel.Settings.StorageFileLocation;
}
