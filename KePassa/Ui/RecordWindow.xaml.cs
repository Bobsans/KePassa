using System.Windows;
using SecretStore.Core;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class RecordWindow {
    private readonly RecordWindowViewModel _model;
    private readonly RecordManager _recordManager;

    public RecordWindow(
        RecordWindowViewModel viewModel,
        RecordManager recordManager
    ) {
        DataContext = _model = viewModel;
        _recordManager = recordManager;

        Owner = Application.Current.MainWindow;
        InitializeComponent();
    }

    public void SetRecord(RecordViewModel record) {
        _model.Record = record;
    }

    public void SetParentRecord(RecordViewModel record) {
        Console.WriteLine($"PARENT {record.Id}");
        _model.Record.ParentId = record.Id;
    }

    private void ButtonSaveOnClick(object sender, RoutedEventArgs e) {
        _recordManager.Save(_model.Record);
        Close();
    }

    private void ButtonCancelOnClick(object sender, RoutedEventArgs e) {
        Close();
    }
}
