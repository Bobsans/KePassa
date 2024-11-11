using KePassa.Core.Abstraction;
using MessagePack;

namespace KePassa.Core.Data;

[MessagePackObject(true), Serializable]
public class Record : IRecord {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
