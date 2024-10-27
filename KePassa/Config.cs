using System.IO;

namespace SecretStore;

public static class Config {
    public static string DataDirectoryPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KePassa");
    public static string SettingsFilePath => Path.Combine(DataDirectoryPath, "settings.dat");
    public static string DefaultStorageFilePath => Path.Combine(DataDirectoryPath, "storage.sse");
}
