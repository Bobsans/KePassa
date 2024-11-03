using System.Windows;
using KePassa.Core.Data;
using SecretStore.Core;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class RecordCategoryWindow {
    private readonly RecordCategoryModel _model;
    private readonly RecordManager _recordManager;

    private Guid? _parentId;

    public RecordCategoryWindow(RecordManager recordManager) {
        DataContext = _model = new RecordCategoryModel();
        _recordManager = recordManager;

        Owner = Application.Current.MainWindow;

        InitializeComponent();
    }

    public RecordCategoryWindow WithData(RecordCategoryModel value) {
        _model.Update(value);
        return this;
    }

    public RecordCategoryWindow WithParentId(Guid? parentId) {
        _parentId = parentId;
        return this;
    }

    private void ButtonSaveOnClick(object sender, RoutedEventArgs e) {
        _recordManager.AddOrUpdate<RecordCategory>(_model.Id, _parentId, it => {
            it.Name = _model.Name;
            it.Description = _model.Description;
        });
        Close();
    }

    private void ButtonCancelOnClick(object sender, RoutedEventArgs e) {
        Close();
    }
}
