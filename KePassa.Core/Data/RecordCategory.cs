using KePassa.Core.Abstraction;
using MessagePack;

namespace KePassa.Core.Data;

[MessagePackObject, Serializable]
public class RecordCategory: IRecord {
    [Key("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Key("name")]
    public string Name { get; set; } = string.Empty;

    [Key("description")]
    public string Description { get; set; } = string.Empty;

    [Key("children")]
    public List<IRecord> Children { get; set; } = [];
}
