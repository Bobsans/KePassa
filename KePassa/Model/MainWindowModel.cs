using System.Collections.ObjectModel;
using System.Windows;
using DimTim.DependencyInjection;
using KePassa.Core.Abstraction;
using SecretStore.Core;
using SecretStore.Ui;
using SecretStore.Ui.Helper;

namespace SecretStore.Model;

public class MainWindowModel : BaseModel, IDisposable {
    private readonly RecordManager _recordManager;

    public DelegateCommand OpenSettingsCommand { get; }
    public DelegateCommand ExitCommand { get; }
    public DelegateCommand AddCategoryCommand { get; }
    public DelegateCommand AddRecordCommand { get; }
    public DelegateCommand EditRecordCommand { get; }
    public DelegateCommand DeleteRecordCommand { get; }

    public ObservableCollection<IRecordModel> Records { get; } = [];
    public IRecordModel? SelectedRecord { get; set; }

    public MainWindowModel(IScope scope, RecordManager recordManager) {
        _recordManager = recordManager;

        OpenSettingsCommand = new DelegateCommand(_ => scope.Resolve<SettingsWindow>().Show());
        ExitCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        AddCategoryCommand = new DelegateCommand(_ => { Console.WriteLine("CATEGORY"); });
        AddRecordCommand = new DelegateCommand(_ => {
            var window = scope.Resolve<RecordWindow>();
            if (SelectedRecord is not null) {
                window.SetParentId(SelectedRecord.Id);
            }

            window.Show();
        });
        EditRecordCommand = new DelegateCommand(_ => {
            var window = scope.Resolve<RecordWindow>();
            if (SelectedRecord is RecordModel model) {
                window.SetRecord(model);
            }

            window.Show();
        });
        DeleteRecordCommand = new DelegateCommand(_ => { });

        _recordManager.OnReload += OnReload;
        _recordManager.OnChanged += OnRecordChanged;
    }

    private void OnReload() {
        Records.Clear();
        foreach (var record in _recordManager.Records) {
            Records.Add(IRecordModel.From(record));
        }

        OnPropertyChanged(nameof(Records));
    }

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
            }
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
        GC.SuppressFinalize(this);
    }
}
