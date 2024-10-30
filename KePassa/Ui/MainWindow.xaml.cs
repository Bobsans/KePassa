using System.Windows;
using System.Windows.Input;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class MainWindow {
    private readonly MainWindowViewModel _model;
    private readonly IScope _scope;

    public MainWindow(
        MainWindowViewModel viewModel,
        RecordManager recordManager,
        IScope scope
    ) {
        DataContext = _model = viewModel;

        _scope = scope;

        recordManager.Load();

        InitializeComponent();
    }

    private void OpenStorageCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.OpenStorageCommand.Execute(e);
    private void SettingsCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.OpenSettingsCommand.Execute(e);
    private void ExitCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.ExitCommand.Execute(e);
    private void AddRecordCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.AddRecordCommand.Execute(e);
    private void EditRecordCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.EditRecordCommand.Execute(e);
    private void DeleteRecordCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.DeleteRecordCommand.Execute(e);

    private void TreeViewRecordsOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        _model.SelectedRecord = e.NewValue as RecordViewModel;
    }

    private void TreeViewRecordsOnDoubleClick(object sender, MouseButtonEventArgs e) {
        _model.EditRecordCommand.Execute(e);
    }
}
