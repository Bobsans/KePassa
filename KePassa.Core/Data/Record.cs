using KePassa.Core.Abstraction;
using MessagePack;

namespace KePassa.Core.Data;

[MessagePackObject, Serializable]
public class Record: IRecord {
    [Key("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Key("name")]
    public string Name { get; set; } = string.Empty;

    [Key("description")]
    public string Description { get; set; } = string.Empty;

    [Key("content")]
    public string Content { get; set; } = string.Empty;
}
