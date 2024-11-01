using System.Windows;
using KePassa.Core.Data;
using SecretStore.Core;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class RecordWindow {
    private RecordModel _model;
    private readonly RecordManager _recordManager;

    private Guid? _parentId;

    public RecordWindow(
        RecordManager recordManager
    ) {
        DataContext = _model = new RecordModel();
        _recordManager = recordManager;

        Owner = Application.Current.MainWindow;
        InitializeComponent();
    }

    public void SetRecord(RecordModel record) {
        _model = record;
    }

    public void SetParentId(Guid? parentId) {
        _parentId = parentId;
    }

    private void ButtonSaveOnClick(object sender, RoutedEventArgs e) {
        _recordManager.AddOrUpdate<Record>(_model.Id, _parentId, it => {
            it.Name = _model.Name;
            it.Description = _model.Description;
            it.Content = _model.Content;
        });
        Close();
    }

    private void ButtonCancelOnClick(object sender, RoutedEventArgs e) {
        Close();
    }
}
