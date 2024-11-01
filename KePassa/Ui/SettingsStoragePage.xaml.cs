using System.Windows;
using Microsoft.Win32;
using SecretStore.Model;

namespace SecretStore.Ui;

public partial class SettingsStoragePage {
    private readonly SettingsStoragePageModel _model;

    public SettingsStoragePage(
        SettingsStoragePageModel model
    ) {
        DataContext = _model = model;
        InitializeComponent();
    }

    private void ButtonChangeOnClick(object sender, RoutedEventArgs e) {
        var dialog = new SaveFileDialog {
            DefaultExt = Config.StorageFileExtension,
            AddExtension = true
        };
        var result = dialog.ShowDialog();

        if (result.HasValue && result.Value) {
            _model.StorageFileLocation = dialog.FileName;
        }
    }
}
