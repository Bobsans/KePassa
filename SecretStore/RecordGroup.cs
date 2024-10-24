using MessagePack;

namespace SecretStore;

[MessagePackObject]
public class RecordGroup {
    [Key("name")]
    public string Name { get; set; } = string.Empty;

    [Key("description")]
    public string Description { get; set; } = string.Empty;

    [Key("records")]
    public List<Record> Records { get; set; } = [];
}
