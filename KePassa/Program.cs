using DimTim.DependencyInjection;
using DimTim.Logging;
using SecretStore.Core;
using SecretStore.Data;
using SecretStore.Ui;
using SecretStore.Ui.Model;

namespace SecretStore;

public static class Program {
    [STAThread]
    public static void Main(string[] args) {
        App.Services = ConfigureServices();
        App.Services.Resolve<App>().Run();
    }

    private static IContainer ConfigureServices() {
        var builder = new ContainerBuilder();
        builder.Singleton<ILogger>(new ConsoleLogger { UseColors = true });
        builder.Singleton<SettingsManager>();
        builder.Singleton<Settings>(scope => scope.Resolve<SettingsManager>().Current);
        builder.Singleton<RecordManager>();

        builder.Singleton<App>();
        builder.Singleton<MainWindow>();
        builder.Transient<RecordWindow>();
        builder.Transient<MasterPasswordWindow>();
        builder.Transient<SettingsWindow>();
        builder.Transient<SettingsGeneralPage>();
        builder.Transient<SettingsStoragePage>();

        builder.Singleton<MainWindowViewModel>();
        builder.Transient<RecordWindowViewModel>();
        builder.Transient<MasterPasswordWindowViewModel>();
        builder.Transient<SettingsWindowViewModel>();
        builder.Transient<SettingsGeneralPageViewModel>();

        return builder.Build();
    }
}
