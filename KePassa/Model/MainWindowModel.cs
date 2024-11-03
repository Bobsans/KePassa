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
        foreach (var record in _recordManager.Records) {
            Records.Add(IRecordModel.From(record));
        }

        OnPropertyChanged(nameof(Records));
    }

    [SuppressPropertyChangedWarnings]
    private void OnRecordChanged(IRecord record, Guid? parentId) {
        var item = FindRecordModel(record.Id, Records);
        if (item is not null) {
            item.Update(record);
        } else {
            if (parentId is not null) {
                var parent = FindRecordModel(parentId.Value, Records);
                if (parent is RecordCategoryModel recordCategoryModel) {
                    recordCategoryModel.Children.Add(IRecordModel.From(record));
                }
            } else {
                Records.Add(IRecordModel.From(record));
            }
        }
    }

    private void OnRecordDeleted(IRecord record, Guid? parentId) {
        if (parentId is not null) {
            var parent = FindRecordModel(parentId.Value, Records);
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
                var found = FindRecordModel(model.Id, recordCategoryModel.Children);
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
