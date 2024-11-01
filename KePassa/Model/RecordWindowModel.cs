namespace SecretStore.Model;

public class RecordWindowModel : BaseModel {
    public Guid? ParentId { get; set; }
    public RecordModel Record { get; set; } = new();
}
