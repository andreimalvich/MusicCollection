using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Core.Initialization;
using MusicCollection.Core.Repo;
using System.Windows;
using System.Windows.Documents;

namespace MusicCollection.TestApp;

public partial class App : Application
{
    

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString =
            @"Server=(localdb)\mssqllocaldb;Database=MusicCollectionDB;Trusted_Connection=True;";
        

        try
        {            
            optionsBuilder.UseSqlServer(connectionString);
            
            using (ApplicationDbContext context = new ApplicationDbContext(optionsBuilder.Options))
            {


            }

            
            //SampleDataInitializer.InitializeData(context);
            
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

