using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DimTim.DependencyInjection;
using DimTim.Logging;
using SecretStore.Core;
using SecretStore.Model;
using SecretStore.Ui.Helper;

namespace SecretStore.Ui;

public partial class MainWindow {
    private readonly MainWindowModel _model;
    private readonly RecordManager _recordManager;
    private readonly IScope _scope;
    private readonly ILogger _logger;

    public MainWindow(
        MainWindowModel model,
        RecordManager recordManager,
        IScope scope,
        ILogger logger
    ) {
        DataContext = _model = model;

        _recordManager = recordManager;
        _scope = scope;
        _logger = logger;

        recordManager.Load();

        InitializeComponent();
    }

    private void SettingsCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.OpenSettingsCommand.Execute(e);
    private void ExitCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) => _model.ExitCommand.Execute(e);

    private void TreeViewRecordsOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        _model.SelectedRecord = e.NewValue as IRecordModel;
    }

    private void MenuItemAddCategoryOnClick(object sender, RoutedEventArgs e) {
        _scope.Resolve<RecordCategoryWindow>().WithParentId(_model.SelectedRecord?.Id).Show();
    }

    private void MenuItemEditCategoryOnClick(object sender, RoutedEventArgs e) {
        if (_model.SelectedRecord is RecordCategoryModel model) {
            _scope.Resolve<RecordCategoryWindow>().WithData(model).Show();
        } else {
            _logger.Error("Selected record is not a record category");
        }
    }

    private void MenuItemAddRecordOnClick(object sender, RoutedEventArgs e) {
        _scope.Resolve<RecordWindow>().Show();
    }

    private void MenuItemEditRecordOnClick(object sender, RoutedEventArgs e) {
        if (_model.SelectedRecord is RecordModel model) {
            _scope.Resolve<RecordWindow>().WithData(model).Show();
        } else {
            _logger.Error("Selected record is not a record");
        }
    }

    private void TreeViewItemContextMenuOnOpened(object sender, RoutedEventArgs e) {
        var menu = sender as ContextMenu;
        if (menu?.PlacementTarget is FrameworkElement element) {
            var item = element.FindAncestor<TreeViewItem>();
            _model.SelectedRecord = item?.DataContext as IRecordModel;
            if (item is not null) {
                item.IsSelected = true;
                item.BringIntoView();
            }
        }
    }

    private void MenuItemDeleteIRecordOnClick(object sender, RoutedEventArgs e) {
        if (_model.SelectedRecord is not null) {
            _recordManager.Delete(_model.SelectedRecord.Id);
        } else {
            _logger.Error("Selected record is not a record");
        }
    }
}
