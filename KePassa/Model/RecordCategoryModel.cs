using System.Collections.ObjectModel;
using KePassa.Core.Abstraction;
using KePassa.Core.Data;

namespace SecretStore.Model;

public class RecordCategoryModel : BaseModel, IRecordModel {
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservableCollection<IRecordModel> Children { get; set; } = [];

    public void Update(RecordCategory category) {
        Id = category.Id;
        ParentId = category.ParentId;
        Name = category.Name;
        Description = category.Description;
    }

    public void Update(RecordCategoryModel category) {
        Id = category.Id;
        ParentId = category.ParentId;
        Name = category.Name;
        Description = category.Description;
    }

    public static RecordCategoryModel From(RecordCategory category, List<IRecord>? all = null) => new() {
        Id = category.Id,
        ParentId = category.ParentId,
        Name = category.Name,
        Description = category.Description,
        Children = new ObservableCollection<IRecordModel>(all?.Where(it => it.ParentId == category.Id).Select(it => IRecordModel.From(it, all)) ?? [])
    };

    public override string ToString() {
        return $"RecordCategoryModel [{Name}]";
    }
}
