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
        SettingManager settingManager
    ) {
        _settingManager = settingManager;

        ShutdownMode = ShutdownMode.OnMainWindowClose;
        Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Ui/Assets/Styles.xaml", UriKind.Relative) });
        Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Ui/Assets/Icons.xaml", UriKind.Relative) });

        // Check data directory
        Directory.CreateDirectory(Config.DataDirectoryPath);
    }

    protected override void OnStartup(StartupEventArgs e) {
        MainWindow = Services.Resolve<MainWindow>();

        base.OnStartup(e);

        if (_settingManager.Current.MasterPasswordHash is null) {
            var passwordWindow = Services.Resolve<MasterPasswordWindow>();
            passwordWindow.Closed += (_, _) => {
                if (_settingManager.Current.MasterPasswordHash is not null) {
                    MainWindow.Show();
                } else {
                    Current.Shutdown();
                }
            };
            passwordWindow.ShowDialog();
        } else {
            MainWindow.Show();
        }
    }
}
