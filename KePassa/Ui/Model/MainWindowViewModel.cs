using System.Collections.ObjectModel;
using System.Windows;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Data;
using SecretStore.Ui.Helper;

namespace SecretStore.Ui.Model;

public class MainWindowViewModel : BaseViewModel, IDisposable {
    private readonly RecordManager _recordManager;

    public DelegateCommand OpenStorageCommand { get; }
    public DelegateCommand OpenSettingsCommand { get; }
    public DelegateCommand ExitCommand { get; }
    public DelegateCommand AddRecordCommand { get; }
    public DelegateCommand EditRecordCommand { get; }
    public DelegateCommand DeleteRecordCommand { get; }

    public ObservableCollection<RecordViewModel> Records { get; } = [];
    public RecordViewModel? SelectedRecord { get; set; }

    public MainWindowViewModel(IScope scope, RecordManager recordManager) {
        _recordManager = recordManager;

        OpenStorageCommand = new DelegateCommand(_ => scope.Resolve<SettingsWindow>().Show());
        OpenSettingsCommand = new DelegateCommand(_ => scope.Resolve<SettingsWindow>().Show());
        ExitCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        AddRecordCommand = new DelegateCommand(_ => {
            var window = scope.Resolve<RecordWindow>();
            if (SelectedRecord is not null) {
                window.SetParentRecord(SelectedRecord);
            }

            window.Show();
        });
        EditRecordCommand = new DelegateCommand(_ => {
            var window = scope.Resolve<RecordWindow>();
            if (SelectedRecord is not null) {
                window.SetRecord(SelectedRecord);
            }

            window.Show();
        });
        DeleteRecordCommand = new DelegateCommand(_ => { });

        _recordManager.OnReload += OnReload;
        _recordManager.OnRecordChanged += OnRecordChanged;
        _recordManager.OnRecordAdded += OnRecordChanged;
    }

    private void OnReload() {
        Records.Clear();
        foreach (var model in _recordManager.Tree) {
            Records.Add(model);
        }

        OnPropertyChanged(nameof(Records));
    }

    private void OnRecordChanged(Record record) {
        var item = FindRecordModel(record.Id, Records);
        if (item is not null) {
            item.Name = record.Name;
            item.Description = record.Description;
            item.Content = record.Content;
        } else {
            if (record.ParentId is not null) {
                var parent = FindRecordModel(record.ParentId.Value, Records);
                parent?.Children.Add(new RecordViewModel {
                    Id = record.Id,
                    Name = record.Name,
                    Description = record.Description,
                    Content = record.Content,
                    ParentId = record.ParentId
                });
            }
        }
    }

    private static RecordViewModel? FindRecordModel(Guid id, IEnumerable<RecordViewModel> source) {
        foreach (var model in source) {
            if (model.Id == id) {
                return model;
            }

            if (model.Children.Count > 0) {
                var found = FindRecordModel(model.Id, model.Children);
                if (found is not null) {
                    return found;
                }
            }
        }

        return null;
    }

    public void Dispose() {
        _recordManager.OnReload -= OnReload;
        _recordManager.OnRecordChanged -= OnRecordChanged;
        _recordManager.OnRecordAdded -= OnRecordChanged;
        GC.SuppressFinalize(this);
    }
}
