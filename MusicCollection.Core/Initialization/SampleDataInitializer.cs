using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MusicCollection.Core.EfStructures;
using MusicCollection.Models.Entities;
using MusicCollection.Models.Entities.Base;

namespace MusicCollection.Core.Initialization;

public static class SampleDataInitializer
{
    public static void ClearData(ApplicationDbContext context)
    {
        var entities = new[]
        {
            typeof(Track).FullName,
            typeof(Album).FullName,
            typeof(PhysicalDisc).FullName,
            typeof(Artist).FullName,
            typeof(AlbumImage).FullName,
        };

        foreach (var entityName in entities)
        {
            var entity = context.Model.FindEntityType(entityName);
            var tableName = entity.GetTableName();
            var schemaName = entity.GetSchema();
            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
        }
    }

    public static List<Artist> GetArtists() =>
    [
        new Artist
        {
            Name = "Pink Floyd",
            Albums = new List<Album>
            {
                new Album
                {
                    Title = "The Wall",
                    ReleaseYear = 1979,
                    Label = "Columbia",
                    CatalogNumber = "PC2 36183",
                    Packaging = Format.Digipack,
                    Discs = new List<PhysicalDisc>
                    {
                        new PhysicalDisc
                        {
                            DiscNumber = 1,
                            Tracks = new List<Track>
                            {
                                new Track { Number=1, Title = "In the Flesh?", Duration = new TimeSpan(0, 3, 16) },
                                new Track { Number=2, Title = "The Thin Ice", Duration = new TimeSpan(0, 2, 27) },
                            }
                        }
                    }
                },
                new Album
                {
                    Title = "The Dark Side of the Moon",
                    ReleaseYear = 1973,
                    Label = "Harvest",
                    CatalogNumber = "SHVL 804",
                    Packaging = Format.JewelCase,
                    Discs = new List<PhysicalDisc>
                    {
                        new PhysicalDisc
                        {
                            DiscNumber = 1,
                            Tracks = new List<Track>
                            {
                                new Track { Number = 1, Title = "Speak to Me", Duration = new TimeSpan(0, 1, 30) },
                                new Track { Number = 2, Title = "Breathe", Duration = new TimeSpan(0, 2, 43) }
                            }
                        }
                    }

                },
            },
        },
        new Artist
        {
            Name = "At The Gates",
            Albums = new List<Album>
            {
                new Album
                {
                    Title = "The Ghost of a Future Dead",
                    ReleaseYear = 2026,
                    Label = "Century Media Records",
                    CatalogNumber = "1 94398 64942 9",
                    Packaging = Format.JewelCase,
                    Discs = new List<PhysicalDisc>
                    {
                        new PhysicalDisc
                        {
                            DiscNumber = 1,
                            Tracks = new List<Track>
                            {
                                new Track { Number=1, Title = "The Fever Mask", Duration = new TimeSpan(0, 3, 12) },
                                new Track { Number=2, Title = "The Dissonant Void", Duration = new TimeSpan(0, 2, 47) },
                            }

                        }

                    }
                }

            },
        },
        new Artist
        {
            Name = "Accept",
            Albums = new List<Album>
            {
                new Album
                {
                    Title = "Balls to the Wall",
                    ReleaseYear = 1983,
                    Label = "RCA",
                    CatalogNumber = "035627018619",
                    Packaging = Format.JewelCase,
                    Discs = new List<PhysicalDisc>
                    {
                        new PhysicalDisc
                        {
                            DiscNumber= 1,
                            Tracks = new List<Track>
                            {
                                new Track { Number=1, Title = "Balls to the Wall", Duration = new TimeSpan(0, 5, 42) },
                                new Track { Number=2, Title = "London Leatherboys", Duration = new TimeSpan(0, 3, 57) },
                            }
                        }
                    }
                },
                new Album
                {
                    Title = "Staying a Life",
                    ReleaseYear = 1990,
                    Label = "RCA",
                    CatalogNumber = "0035627472015",
                    Packaging = Format.JewelCase,
                    Discs = new List<PhysicalDisc>
                    {
                        new PhysicalDisc
                        {
                            DiscNumber = 1,
                            Tracks = new List<Track>
                            {
                                new Track { Number=1, Title = "Metal Heart", Duration = new TimeSpan(0, 5, 25) },
                                new Track { Number=2, Title = "Breaker", Duration = new TimeSpan(0, 3, 40) },
                            }
                        },
                        new PhysicalDisc
                        {
                            DiscNumber = 2,
                            Tracks = new List<Track>
                            {
                                new Track { Number=1, Title = "Head over Heels", Duration = new TimeSpan(0, 5, 48) },
                                new Track { Number=2, Title = "Guitar Solo Wolf", Duration = new TimeSpan(0, 4, 27) },
                            }
                        }
                    }
                }
            }
        }
    ];

    public static void SeedData(ApplicationDbContext context)
    {
        try
        {
            ProcessInsert(context, context.Artists!, SampleData.Artists);
            ProcessInsert(context, context.Albums!, SampleData.Albums);
            ProcessInsert(context, context.Discs!, SampleData.Discs);
            ProcessInsert(context, context.Tracks!, SampleData.Tracks);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            // Поставить BP чтобы выяснить в чем проблема
            throw;
        }

        static void ProcessInsert<TEntity>(ApplicationDbContext context, DbSet<TEntity> table,
            List<TEntity> records) where TEntity : BaseEntity
        {
            if (table.Any())
            {
                return;
            }

            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var metaData = context.Model.FindEntityType(typeof(TEntity).FullName);
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON");
                    table.AddRange(records);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw(
                        $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF");
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            });
        }





    }

    public static void InitializeDataII(ApplicationDbContext context)
    {
        // Применяем все существующие миграции к базе данных
        context.Database.Migrate();

        // Если в таблице артистов уже есть записи, значит засев не требуется
        if (context.Artists.Any())
        {
            return;
        }

        // Добавляем данные из SampleData
        context.Artists.AddRange(GetArtists());

        // Сохраняем все изменения одной транзакцией
        context.SaveChanges();
    }
    
    internal static void DropAndCreateDatabase(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    public static void InitializeData(ApplicationDbContext context)
    {
        DropAndCreateDatabase(context);
        SeedData(context);
    }

    public static void ClearAndReseedDatabase(ApplicationDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }
}
