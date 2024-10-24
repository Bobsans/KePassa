using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SecretStore;

public partial class MainWindow : Window {
    public new MainDataContext DataContext { get; set; } = new();

    public MainWindow() {
        InitializeComponent();
        DataContext.Groups.Add(new RecordGroup {
            Name = "Github",
            Records = [
                new Record { Name = "ILVO", Description = "Ilvo servers key", Content = "5492759432598646329875643782654873265" }
            ]
        });
        DataContext.Groups.Add(new RecordGroup {
            Name = "Gitlab",
            Records = [
                new Record { Name = "ILVO", Description = "Ilvo servers key", Content = "5492759432598646329875643782654873265" },
                new Record { Name = "ILVO1", Description = "Ilvo servers key", Content = "5492759432598646329875643782654873265" },
                new Record { Name = "ILVO2", Description = "Ilvo servers key", Content = "5492759432598646329875643782654873265" }
            ]
        });

        Sidebar.ItemsSource = DataContext.Groups;

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
                new RecordManager().Save(DataContext.Groups);
            } else if (Sidebar.SelectedItem is Record record) {
                record.Name = TextBoxRecordName.Text.Trim();
                record.Description = TextBoxRecordDescription.Text.Trim();
                record.Content = TextBoxRecordContent.Text;
                new RecordManager().Save(DataContext.Groups);
            }
        };

        ButtonAddGroup.Click += (s, e) => {
            DataContext.Groups.Add(new RecordGroup());
        };

        ButtonAddRecord.Click += (s, e) => {
            if (Sidebar.SelectedItem is RecordGroup group) {
                group.Records.Add(new Record());
            }
        };

        if (File.Exists("storage.sse")) {
            var items = new RecordManager().Load();
            DataContext.Groups.Clear();
            foreach (var group in items) {
                DataContext.Groups.Add(group);
            }
        } else {
            new RecordManager().Save(DataContext.Groups.ToList());
        }
    }
}
