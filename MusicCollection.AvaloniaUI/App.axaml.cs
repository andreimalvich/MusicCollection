using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MusicCollection.AvaloniaUI.ViewModels;
using MusicCollection.AvaloniaUI.Views;
using MusicCollection.Core.EfStructures;
using System;
using System.IO;
using System.Linq;

namespace MusicCollection.AvaloniaUI;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;    

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
                //throw new InvalidOperationException(
                //"Критическая ошибка: База данных MusicCollectionDB недоступна. " +
                //"Убедитесь, что SQL Server запущен и строка подключения в appsettings.json верна.");
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