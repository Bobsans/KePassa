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

    public ObservableCollection<RecordViewModel> Records => new(_recordManager.Tree);

    public MainWindowViewModel(IScope scope, RecordManager recordManager) {
        _recordManager = recordManager;

        OpenStorageCommand = new DelegateCommand(_ => scope.Resolve<SettingsWindow>().Show(), _ => true);
        OpenSettingsCommand = new DelegateCommand(_ => scope.Resolve<SettingsWindow>().Show(), _ => true);
        ExitCommand = new DelegateCommand(_ => Application.Current.Shutdown(), _ => true);

        _recordManager.OnReload += OnReload;
        _recordManager.OnRecordAdded += OnRecordsChanged;
        _recordManager.OnRecordChanged += OnRecordsChanged;
    }

    private void OnRecordsChanged(Record _) => OnPropertyChanged(nameof(Records));
    public void OnReload() => OnPropertyChanged(nameof(Records));

    public void Dispose() {
        _recordManager.OnReload -= OnReload;
        _recordManager.OnRecordAdded -= OnRecordsChanged;
        _recordManager.OnRecordChanged -= OnRecordsChanged;
        GC.SuppressFinalize(this);
    }
}
