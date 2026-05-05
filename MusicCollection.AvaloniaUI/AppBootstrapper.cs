using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicCollection.AvaloniaUI.ViewModels;
using MusicCollection.AvaloniaUI.Views;
using MusicCollection.Core.EfStructures;
using System;
using System.IO;

namespace MusicCollection.AvaloniaUI;

public class AppBootstrapper
{
    private IConfiguration? _configuration;
    
    public void Initialize()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        var connectionString = _configuration?.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Строка подключения не найдена в appsettings.json");
        
        services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MainWindow>();

        return services.BuildServiceProvider();
    }

    public bool IsDatabaseAvailable(IServiceProvider serviceProvider)
    {
        try
        {
            var factory = serviceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            using var context = factory.CreateDbContext();
            return context.Database.CanConnect();
        }
        catch
        {
            return false;
        }
    }
}
