using System.Windows;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Initialization;

namespace MusicCollection.TestApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            var contextFactory = new ApplicationDbContextFactory();
            using (var context = contextFactory.CreateDbContext(Array.Empty<string>()))
            {
                SampleDataInitializer.InitializeData(context);
            }
        }
        catch (Exception ex) 
        { 
            MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}",
                            "Критическая ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
            Shutdown();
        }
    }
}

