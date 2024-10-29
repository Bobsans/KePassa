using System.IO;
using System.Windows;
using DimTim.DependencyInjection;
using SecretStore.Core;
using SecretStore.Ui;

namespace SecretStore;

public class App : Application {
    private readonly SettingManager _settingManager;

    public static IContainer Services { get; set; } = null!;

    public App(
        MainWindow mainWindow,
        SettingManager settingManager
    ) {
        _settingManager = settingManager;

        MainWindow = mainWindow;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Ui/Styles.xaml", UriKind.Relative) });
    }

    protected override void OnStartup(StartupEventArgs e) {
        // Check data directory
        Directory.CreateDirectory(Config.DataDirectoryPath);

        base.OnStartup(e);

        if (_settingManager.Current.MasterPasswordHash is null) {
            var passwordWindow = Services.Resolve<MasterPasswordWindow>();
            passwordWindow.Closed += (_, _) => {
                if (_settingManager.Current.MasterPasswordHash is not null) {
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
