using System.Windows;
using System.Windows.Input;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class MainWindow {
    private readonly IScope _scope;
    private readonly MainWindowViewModel _model;

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

    private void OpenStorageCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) {
        _model.OpenStorageCommand.Execute(sender);
    }

    private void SettingsCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) {
        _model.OpenSettingsCommand.Execute(sender);
    }

    private void ExitCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) {
        _model.ExitCommand.Execute(sender);
    }

    private void MenuItemAddOnClick(object sender, RoutedEventArgs e) {
        var window = _scope.Resolve<RecordWindow>();
        window.Owner = this;
        if (TreeViewRecords.SelectedValue is RecordViewModel record) {
            window.SetParent(record);
        }

        window.Show();
    }
}
