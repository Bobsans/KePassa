using MessagePack;

namespace SecretStore.Data;

[MessagePackObject]
public class Settings {
    [Key("storage_file_location")]
    public required string StorageFileLocation { get; set; }
    [Key("master_password_hash")]
    public required byte[]? MasterPasswordHash { get; set; }

    public Settings Clone() {
        return MessagePackSerializer.Deserialize<Settings>(MessagePackSerializer.Serialize(this));
    }
}
