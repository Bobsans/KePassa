using System.Windows;
using System.Windows.Input;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class MainWindow {
    private readonly MainWindowModel _model;
    private readonly IScope _scope;

    public MainWindow(
        MainWindowModel model,
        RecordManager recordManager,
        IScope scope
    ) {
        DataContext = _model = model;

        _scope = scope;

        recordManager.Load();

        InitializeComponent();
    }

    private void SettingsCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.OpenSettingsCommand.Execute(e);
    private void ExitCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.ExitCommand.Execute(e);
    private void AddRecordCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.AddRecordCommand.Execute(e);
    private void EditRecordCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.EditRecordCommand.Execute(e);
    private void DeleteRecordCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.DeleteRecordCommand.Execute(e);

    private void TreeViewRecordsOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        _model.SelectedRecord = e.NewValue as RecordModel;
    }

    private void MenuItemAddCategoryOnClick(object sender, RoutedEventArgs e) {
        _scope.Resolve<RecordCategoryWindow>().Show();
    }
}
