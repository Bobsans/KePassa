using KePassa.Core.Data;
using MessagePack;

namespace KePassa.Core.Abstraction;

[Union(0, typeof(Record)), Union(1, typeof(RecordCategory))]
public interface IRecord {
    Guid Id { get; set; }
    Guid? ParentId { get; set; }
    string Name { get; set; }
    string Description { get; set; }
}
