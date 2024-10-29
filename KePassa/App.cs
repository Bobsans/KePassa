using System.IO;
using System.Windows;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Ui;

namespace SecretStore;

public class App : Application {
    private readonly SettingsManager _settingsManager;

    public static IContainer Services { get; set; } = null!;

    public App(
        MainWindow mainWindow,
        SettingsManager settingsManager
    ) {
        _settingsManager = settingsManager;

        MainWindow = mainWindow;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Ui/Styles.xaml", UriKind.Relative) });
    }

    protected override void OnStartup(StartupEventArgs e) {
        // Check data directory
        Directory.CreateDirectory(Config.DataDirectoryPath);
        _settingsManager.Load();

        base.OnStartup(e);

        if (_settingsManager.Current.MasterPasswordHash is null) {
            var passwordWindow = Services.Resolve<MasterPasswordWindow>();
            passwordWindow.Closed += (_, _) => {
                if (_settingsManager.Current.MasterPasswordHash is not null) {
                    MainWindow!.Show();
                } else {
                    Current.Shutdown();
                }
            };
            passwordWindow.ShowDialog();
        } else {
            MainWindow!.Show();
        }
    }
}
