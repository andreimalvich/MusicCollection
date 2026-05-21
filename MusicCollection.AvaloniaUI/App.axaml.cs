using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MusicCollection.AvaloniaUI.ViewModels;
using MusicCollection.AvaloniaUI.Views;

namespace MusicCollection.AvaloniaUI;

public partial class App : Application
{
    public static System.IServiceProvider ServiceProvider { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var bootstrapper = new AppBootstrapper();
        bootstrapper.Initialize();
        ServiceProvider = bootstrapper.CreateServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (!bootstrapper.IsDatabaseAvailable(ServiceProvider))
            {
                // throw new InvalidOperationException(
                // "Критическая ошибка: База данных MusicCollectionDB недоступна. " +
                // "Убедитесь, что SQL Server запущен и строка подключения в appsettings.json верна.");
                var errorWin = new ErrorWindow();
                errorWin.SetMessage("Критическая ошибка: База данных MusicCollectionDB недоступна. " +
                    "Проверьте настройки SQL Server.");
                desktop.MainWindow = errorWin;
            }
            else
            {
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = mainWindow;
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}
