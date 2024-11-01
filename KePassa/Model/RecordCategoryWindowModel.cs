using KePassa.Core.Data;

namespace SecretStore.Model;

public class RecordCategoryWindowModel : BaseModel {
    public Guid? ParentId { get; set; }
    public RecordCategory Category { get; set; } = new();
}
