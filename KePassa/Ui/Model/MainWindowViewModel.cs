using System.Collections.ObjectModel;
using System.Windows;
using DimTim.DependencyInjection;
using SecretStore.Ui.Helper;

namespace SecretStore.Ui.Model;

public class MainWindowViewModel(IScope scope) : BaseViewModel {
    private RecordViewModel? _selectedRecord;

    public DelegateCommand OpenStorageCommand { get; } = new(_ => scope.Resolve<SettingsWindow>().Show(), _ => true);
    public DelegateCommand OpenSettingsCommand { get; } = new(_ => scope.Resolve<SettingsWindow>().Show(), _ => true);
    public DelegateCommand ExitCommand { get; } = new(_ => Application.Current.Shutdown(), _ => true);

    public ObservableCollection<RecordViewModel> Records { get; set; } = [];

    public RecordViewModel? SelectedRecord {
        get => _selectedRecord;
        set => SetField(ref _selectedRecord, value);
    }
}
