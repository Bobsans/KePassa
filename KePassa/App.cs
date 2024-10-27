using System.IO;
using System.Windows;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Ui;

namespace SecretStore;

public class App: Application {
    public static IContainer Services { get; set; } = null!;

    public App(MainWindow mainWindow, SettingsManager settingsManager) {
        MainWindow = mainWindow;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Ui/Styles.xaml", UriKind.Relative) });

        // Check data directory
        Directory.CreateDirectory(Config.DataDirectoryPath);

        settingsManager.Load();
    }

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);
        MainWindow!.Show();
    }
}
