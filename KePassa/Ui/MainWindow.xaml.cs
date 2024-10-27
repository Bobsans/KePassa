using System.IO;
using System.Windows.Input;
using SecretStore.Core;
using SecretStore.Data;
using SecretStore.Ui.Model;

namespace SecretStore.Ui;

public partial class MainWindow {
    public MainWindow(RecordManager recordManager, MainWindowViewModel windowViewModel) {
        DataContext = windowViewModel;

        InitializeComponent();

        Sidebar.SelectedItemChanged += (s, e) => {
            if (e.NewValue is RecordGroup group) {
                TextBoxRecordName.Text = group.Name;
                TextBoxRecordDescription.Text = group.Description;
                TextBoxRecordContent.Text = string.Empty;
            } else if (e.NewValue is Record record) {
                TextBoxRecordName.Text = record.Name;
                TextBoxRecordDescription.Text = record.Description;
                TextBoxRecordContent.Text = record.Content;
            }
        };

        ButtonSave.Click += (s, e) => {
            if (Sidebar.SelectedItem is RecordGroup group) {
                group.Name = TextBoxRecordName.Text.Trim();
                group.Description = TextBoxRecordDescription.Text.Trim();
                recordManager.Save(windowViewModel.Groups.Select(it => it.ToRecordGroup()), "password");
            } else if (Sidebar.SelectedItem is Record record) {
                record.Name = TextBoxRecordName.Text.Trim();
                record.Description = TextBoxRecordDescription.Text.Trim();
                record.Content = TextBoxRecordContent.Text;
                recordManager.Save(windowViewModel.Groups.Select(it => it.ToRecordGroup()), "password");
            }
        };

        ButtonAddGroup.Click += (s, e) => { windowViewModel.Groups.Add(new RecordGroupViewModel()); };

        ButtonAddRecord.Click += (s, e) => {
            if (Sidebar.SelectedItem is RecordGroup group) {
                group.Records.Add(new Record());
            }
        };

        if (File.Exists("storage.sse")) {
            var items = recordManager.Load("password");
            windowViewModel.Groups.Clear();
            foreach (var group in items) {
                windowViewModel.Groups.Add(RecordGroupViewModel.From(group));
            }
        } else {
            recordManager.Save(windowViewModel.Groups.Select(it => it.ToRecordGroup()), "password");
        }
    }

    private void OpenStorageCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) {
        (DataContext as MainWindowViewModel)!.OpenStorageCommand.Execute(sender);
    }

    private void SettingsCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) {
        (DataContext as MainWindowViewModel)!.OpenSettingsCommand.Execute(sender);
    }

    private void ExitCommandBindingExecuted(object sender, ExecutedRoutedEventArgs e) {
        (DataContext as MainWindowViewModel)!.ExitCommand.Execute(sender);
    }
}
