using KePassa.Core.Abstraction;
using KePassa.Core.Data;

namespace SecretStore.Model;

public interface IRecordModel {
    Guid Id { get; set; }

    public static IRecordModel From(IRecord record) => record switch {
        Record r => RecordModel.From(r),
        RecordCategory rc => RecordCategoryModel.From(rc),
        _ => throw new Exception("Unknown record type")
    };
}

public static class RecordModelExtension {
    public static IRecordModel Update(this IRecordModel model, IRecord record) {
        switch (model) {
            case RecordModel recordModel when record is Record rec:
                recordModel.Update(rec);
                break;
            case RecordCategoryModel recordCategoryModel when record is RecordCategory recordCategory:
                recordCategoryModel.Update(recordCategory);
                break;
            default:
                throw new Exception("Invalid record types");
        }

        return model;
    }
}
