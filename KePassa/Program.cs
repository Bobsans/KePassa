using DimTim.DependencyInjection;
using DimTim.Logging;
using KePassa.Core.Services;
using SecretStore.Core;
using SecretStore.Data;
using SecretStore.Model;
using SecretStore.Ui;

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
        builder.Singleton<Serializer>();
        builder.Singleton<SettingManager>();
        builder.Singleton<Settings>(scope => scope.Resolve<SettingManager>().Current);
        builder.Singleton<RecordManager>();

        builder.Singleton<App>();
        builder.Singleton<MainWindow>();
        builder.Transient<RecordWindow>();
        builder.Transient<RecordCategoryWindow>();
        builder.Transient<MasterPasswordWindow>();
        builder.Transient<SettingsWindow>();
        builder.Transient<SettingsGeneralPage>();
        builder.Transient<SettingsStoragePage>();

        builder.Singleton<MainWindowModel>();
        builder.Transient<RecordWindowModel>();
        builder.Transient<MasterPasswordWindowModel>();
        builder.Scoped<SettingsWindowModel>();
        builder.Transient<SettingsGeneralPageModel>();
        builder.Transient<SettingsStoragePageModel>();

        return builder.Build();
    }
}
