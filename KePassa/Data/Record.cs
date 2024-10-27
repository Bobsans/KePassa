using MessagePack;

namespace SecretStore.Data;

[MessagePackObject]
public class Record {
    [Key("name")]
    public string Name { get; set; } = string.Empty;

    [Key("description")]
    public string Description { get; set; } = string.Empty;

    [Key("content")]
    public string Content { get; set; } = string.Empty;
}
