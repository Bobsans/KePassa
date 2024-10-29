using System.IO;
using DimTim.Logging;
using MessagePack;
using SecretStore.Data;

namespace SecretStore.Core;

public class SettingManager(ILogger logger) {
    private Settings? _settings;

    public bool IsSettingsExist => File.Exists(Config.SettingsFilePath);
    public Settings Current => _settings ?? Load();

    public Settings Load() {
        if (IsSettingsExist) {
            using var stream = File.OpenRead(Config.SettingsFilePath);
            try {
                _settings = MessagePackSerializer.Deserialize<Settings>(stream);
                logger.Info($"Loaded settings from {Config.SettingsFilePath}");
                return _settings;
            } catch (Exception ex) {
                logger.Error(ex, "Unable to load settings");
            }
        } else {
            logger.Info($"No settings file found at {Config.SettingsFilePath}");
        }

        return GetDefault();
    }

    public void Save(Settings settings) {
        using var stream = File.OpenWrite(Config.SettingsFilePath);
        MessagePackSerializer.Serialize(stream, settings);
    }

    public static Settings GetDefault() {
        return new Settings {
            StorageFileLocation = Config.DefaultStorageFilePath,
            MasterPasswordHash = null
        };
    }
}
