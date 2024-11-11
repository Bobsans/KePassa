using System.Windows;
using KePassa.Core.Data;
using SecretStore.Core;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class RecordWindow {
    private readonly RecordModel _model;
    private readonly RecordManager _recordManager;

    private Guid? _parentId;

    public RecordWindow(
        RecordManager recordManager
    ) {
        DataContext = _model = new RecordModel();
        _recordManager = recordManager;

        Owner = Application.Current.MainWindow;
        Loaded += (_, _) => { TextBoxRecordName.Focus(); };

        InitializeComponent();
    }

    public RecordWindow WithData(RecordModel record) {
        _model.Update(record);
        return this;
    }

    public RecordWindow WithParentId(Guid? parentId) {
        _parentId = parentId;
        return this;
    }

    private void ButtonSaveOnClick(object sender, RoutedEventArgs e) {
        _recordManager.AddOrUpdate<Record>(_model.Id, it => {
            it.ParentId = _model.ParentId ?? _parentId;
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
