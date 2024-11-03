using System.Collections.ObjectModel;
using KePassa.Core.Data;

namespace SecretStore.Model;

public class RecordCategoryModel : BaseModel, IRecordModel {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservableCollection<IRecordModel> Children { get; set; } = [];

    public void Update(RecordCategory category) {
        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
    }

    public void Update(RecordCategoryModel category) {
        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
    }

    public static RecordCategoryModel From(RecordCategory category) => new() {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description,
        Children = new ObservableCollection<IRecordModel>(category.Children.Select(IRecordModel.From))
    };

    public override string ToString() {
        return $"RecordCategoryModel [{Name}]";
    }
}
