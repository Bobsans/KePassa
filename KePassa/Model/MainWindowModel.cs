using System.Collections.ObjectModel;
using System.Windows;
using DimTim.DependencyInjection;
using KePassa.Core.Abstraction;
using PropertyChanged;
using SecretStore.Core;
using SecretStore.Ui;
using SecretStore.Ui.Helper;

namespace SecretStore.Model;

public class MainWindowModel : BaseModel, IDisposable {
    private readonly RecordManager _recordManager;

    public DelegateCommand OpenSettingsCommand { get; }
    public DelegateCommand ExitCommand { get; }

    public ObservableCollection<IRecordModel> Records { get; } = [];
    public IRecordModel? SelectedRecord { get; set; }

    public MainWindowModel(IScope scope, RecordManager recordManager) {
        _recordManager = recordManager;

        OpenSettingsCommand = new DelegateCommand(_ => scope.Resolve<SettingsWindow>().Show());
        ExitCommand = new DelegateCommand(_ => Application.Current.Shutdown());

        _recordManager.OnReload += OnReload;
        _recordManager.OnChanged += OnRecordChanged;
        _recordManager.OnDeleted += OnRecordDeleted;
    }

    private void OnReload() {
        Records.Clear();
        foreach (var record in _recordManager.Records.Where(it => it.ParentId is null)) {
            Records.Add(IRecordModel.From(record, _recordManager.Records));
        }

        OnPropertyChanged(nameof(Records));
    }

    [SuppressPropertyChangedWarnings]
    private void OnRecordChanged(IRecord record) {
        var item = FindRecordModel(record.Id, Records);
        if (item is not null) {
            item.Update(record);
        } else {
            if (record.ParentId is not null) {
                var parent = FindRecordModel(record.ParentId.Value, Records);
                if (parent is RecordCategoryModel recordCategoryModel) {
                    recordCategoryModel.Children.Add(IRecordModel.From(record));
                }
            } else {
                Records.Add(IRecordModel.From(record));
            }
        }
    }

    private void OnRecordDeleted(IRecord record) {
        if (record.ParentId is not null) {
            var parent = FindRecordModel(record.ParentId.Value, Records);
            if (parent is RecordCategoryModel category) {
                category.Children.Remove(category.Children.First(it => it.Id == record.Id));
            }
        } else {
            Records.Remove(Records.First(it => it.Id == record.Id));
        }
    }

    private static IRecordModel? FindRecordModel(Guid id, IEnumerable<IRecordModel> source) {
        foreach (var model in source) {
            if (model.Id == id) {
                return model;
            }

            if (model is RecordCategoryModel { Children.Count: > 0 } recordCategoryModel) {
                var found = FindRecordModel(id, recordCategoryModel.Children);
                if (found is not null) {
                    return found;
                }
            }
        }

        return null;
    }

    public void Dispose() {
        _recordManager.OnReload -= OnReload;
        _recordManager.OnChanged -= OnRecordChanged;
        GC.SuppressFinalize(this);
    }
}
