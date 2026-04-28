using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Models.Entities;


namespace MusicCollection.Core.Initialization;

public class SampleData
{
    public static List<Artist> Artists => new()
    {
        new Artist() {Id = 1, Name = "Pink Floyd"},
        new Artist() {Id = 2, Name = "At The Gates"},
        new Artist() {Id = 3, Name = "Accept"},
    };

    public static List<Album> Albums => new()
    {
        new Album()
        {
            Id = 1,
            ArtistId = 1,
            Title = "The Wall",
            ReleaseYear = 1979,
            CatalogNumber = "PC2 36183",
            Packaging = Format.Digipack,
            Label = "Columbia"
        },
        new Album()
        {
            Id = 2,
            ArtistId = 1,
            Title = "The Dark Side of the Moon",
            ReleaseYear = 1973,
            CatalogNumber = "SHVL 804",
            Packaging = Format.JewelCase,
            Label = "Harvest",
        },
        new Album()
        {
            Id = 3,
            ArtistId = 2,
            Title = "The Ghost of a Future Dead",
            ReleaseYear = 2026,
            Label = "Century Media Records",
            CatalogNumber = "1 94398 64942 9",
            Packaging = Format.JewelCase,
        },
        new Album()
        {
            Id = 4,
            ArtistId = 3,
            Title = "Balls to the Wall",
            ReleaseYear = 1983,
            Label = "RCA",
            CatalogNumber = "035627018619",
            Packaging = Format.JewelCase,
        },
        new Album()
        {
            Id = 5,
            ArtistId = 3,
            Title = "Staying a Life",
            ReleaseYear = 1990,
            Label = "RCA",
            CatalogNumber = "0035627472015",
            Packaging = Format.JewelCase,
        },
    };

    public static List<PhysicalDisc> Discs => new()
    {
        new PhysicalDisc()
        {
            Id = 1,
            AlbumId = 1,
            DiscNumber = 1,
        },
        new PhysicalDisc()
        {
            Id = 2,
            AlbumId = 2,
            DiscNumber = 1,
        },
        new PhysicalDisc()
        {
            Id = 3,
            AlbumId = 3,
            DiscNumber = 1,
        },
        new PhysicalDisc()
        {
            Id = 4,
            AlbumId = 4,
            DiscNumber = 1,
        },
        new PhysicalDisc()
        {
            Id = 5,
            AlbumId = 5,
            DiscNumber = 1,
        },
        new PhysicalDisc()
        {
            Id = 6,
            AlbumId = 5,
            DiscNumber = 2,
        },
    };

    public static List<Track> Tracks => new()
    {
        new Track()
        {
            Id = 1,
            PhysicalDiscId = 1,
            Number = 1,
            Title ="In the Flesh?",
            Duration = new TimeSpan(0, 3, 16),
        },
        new Track()
        {
            Id = 2,
            PhysicalDiscId = 1,
            Number = 2,
            Title ="The Thin Ice",
            Duration = new TimeSpan(0, 2, 27),
        },
        new Track()
        {
            Id = 3,
            PhysicalDiscId = 2,
            Number = 1,
            Title ="Speak to Me",
            Duration = new TimeSpan(0, 1, 30),
        },
        new Track()
        {
            Id = 4,
            PhysicalDiscId = 2,
            Number = 2,
            Title ="Breathe",
            Duration = new TimeSpan(0, 2, 43),
        },
        new Track()
        {
            Id = 5,
            PhysicalDiscId = 3,
            Number = 1,
            Title ="The Fever Mask",
            Duration = new TimeSpan(0, 3, 12),
        },
        new Track()
        {
            Id = 6,
            PhysicalDiscId = 3,
            Number = 2,
            Title ="The Dissonant Void",
            Duration = new TimeSpan(0, 2, 47),
        },
        new Track()
        {
            Id = 7,
            PhysicalDiscId = 4,
            Number = 1,
            Title ="Balls to the Wall",
            Duration = new TimeSpan(0, 5, 42),
        },
        new Track()
        {
            Id = 8,
            PhysicalDiscId = 4,
            Number = 2,
            Title ="London Leatherboys",
            Duration = new TimeSpan(0, 3, 57),
        },
        new Track()
        {
            Id = 9,
            PhysicalDiscId = 5,
            Number = 1,
            Title ="Metal Heart",
            Duration = new TimeSpan(0, 5, 25),
        },
        new Track()
        {
            Id = 10,
            PhysicalDiscId = 5,
            Number = 2,
            Title ="Breaker",
            Duration = new TimeSpan(0, 3, 40),
        },
        new Track()
        {
            Id = 11,
            PhysicalDiscId = 6,
            Number = 1,
            Title ="Head over Heels",
            Duration = new TimeSpan(0, 5, 48),
        },
        new Track()
        {
            Id = 12,
            PhysicalDiscId = 6,
            Number = 2,
            Title ="Guitar Solo Wolf",
            Duration = new TimeSpan(0, 4, 27),
        },
    };

}


