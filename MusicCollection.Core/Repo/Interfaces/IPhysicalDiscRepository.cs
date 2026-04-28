using MusicCollection.Core.Repo.Base;
using MusicCollection.Models.Entities;

namespace MusicCollection.Core.Repo.Interfaces;

public interface IPhysicalDiscRepository : IRepository<PhysicalDisc>
{
    Task<List<PhysicalDisc>> GetByFormatAsync(Format format);
}
