using System.IO;
using System.Windows;
using System.Windows.Input;
using DimTim.Logging;
using SecretStore.Core;
using SecretStore.Data;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class MainWindow {
    private readonly ILogger _logger;
    private readonly MainWindowViewModel _model;

    public MainWindow(
        MainWindowViewModel windowViewModel,
        RecordManager recordManager,
        ILogger logger
    ) {
        DataContext = _model = windowViewModel;
        _logger = logger;

        InitializeComponent();

        TreeViewRecords.SelectedItemChanged += (_, e) => { _model.SelectedRecord = e.NewValue as RecordViewModel; };

        ButtonSave.Click += (s, e) => {
            recordManager.Save(windowViewModel.Records.Select(it => it.ToRecord()), "password");
        };

        if (File.Exists("storage.sse")) {
            var items = recordManager.Load("password");
            windowViewModel.Records.Clear();
            foreach (var group in items) {
                windowViewModel.Records.Add(RecordViewModel.From(group));
            }
        } else {
            recordManager.Save(windowViewModel.Records.Select(it => it.ToRecord()), "password");
        }
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
        _logger.Info($"{sender} :: ${e}");
        _logger.Info($"{TreeViewRecords.SelectedValue}");
        if (TreeViewRecords.SelectedValue is RecordViewModel record) {
            record.Children.Add(new RecordViewModel());
        } else {
            _model.Records.Add(new RecordViewModel());
        }
    }
}
