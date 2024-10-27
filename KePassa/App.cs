using System.Windows;
using DimTim.DependencyInjection;
using SecretStore.Ui;

namespace SecretStore;

public class App: Application {
    public static IContainer Services { get; set; } = null!;

    private readonly MainWindow _mainWindow;

    public App(MainWindow mainWindow) {
        _mainWindow = mainWindow;
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Ui/Styles.xaml", UriKind.Relative) });
    }

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);
        _mainWindow.Show();
    }
}
