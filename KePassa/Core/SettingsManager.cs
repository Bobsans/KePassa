using System.IO;
using MessagePack;
using SecretStore.Data;

namespace SecretStore.Core;

public class SettingsManager(Logger logger) {
    private const string FILE_NAME = "keepassa.dat";

    private Settings? _settings;

    private static string SettingsFilePath => Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FILE_NAME));

    public bool IsSettingsExist => File.Exists(SettingsFilePath);
    public Settings Current => _settings ??= Load();

    public Settings Load() {
        if (IsSettingsExist) {
            using var stream = File.OpenRead(SettingsFilePath);
            try {
                return MessagePackSerializer.Deserialize<Settings>(stream);
            } catch (Exception e) {
                logger.Error($"Unable to load settings", e);
            }
        }

        return GetDefault();
    }

    public void Save(Settings settings) {
        using var stream = File.OpenWrite(SettingsFilePath);
        MessagePackSerializer.Serialize(stream, settings);
    }

    public Settings GetDefault() {
        return new Settings {
            StorageFileLocation = SettingsFilePath,
            MasterPasswordHash = []
        };
    }
}
