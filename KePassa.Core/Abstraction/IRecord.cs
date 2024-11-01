namespace KePassa.Core.Abstraction;

public interface IRecord {
    Guid Id { get; set; }
    string Name { get; set; }
    string Description { get; set; }
}
