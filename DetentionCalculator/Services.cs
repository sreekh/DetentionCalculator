using DetentionCalculator.Core.Databases;
using DetentionCalculator.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetentionCalculator.Core.Services
{
    public interface ICRUDService<T,I>
        where T : DEntity, new()
        where I : IDEntity
    {
        IDEntityList<T,I> Get();
        I Get(Guid id);
        void Add(I entity);
        void AddList(List<I> entityList);
    }
    public interface IOffenceCRUDService : ICRUDService<Offence, IOffence> { }
    public class OffenceService : IOffenceCRUDService
    {
        private IRepository Repository;
        public OffenceService(IRepository repository)
        {
            this.Repository = repository;
        }
        IDEntityList<Offence, IOffence> ICRUDService<Offence, IOffence>.Get()
        {
            return this.Repository.OffenceList;
        }

        IOffence ICRUDService<Offence, IOffence>.Get(Guid id)
        {
            return this.Repository.OffenceList.InternalList.Where(o => o.Id == id).Select(o => (IOffence)o).First();
        }

        void ICRUDService<Offence, IOffence>.Add(IOffence entity)
        {
            this.Repository.OffenceList.InternalList.Add(entity);
        }

        public void AddList(List<IOffence> entityList)
        {
            this.Repository.OffenceList.InternalList.AddRange(entityList);
        }
    }
    public interface IStandardDetentionForOffenceCRUDService : ICRUDService<StandardDetentionForOffence, IStandardDetentionForOffence> { }
    public class StandardDetentionForOffenceService : IStandardDetentionForOffenceCRUDService
    {
        private IRepository Repository;
        public StandardDetentionForOffenceService(IRepository repository)
        {
            this.Repository = repository;
        }
        IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> ICRUDService<StandardDetentionForOffence, IStandardDetentionForOffence>.Get()
        {
            return this.Repository.StandardDetentionForOffenceList;
        }

        IStandardDetentionForOffence ICRUDService<StandardDetentionForOffence, IStandardDetentionForOffence>.Get(Guid id)
        {
            return this.Repository.StandardDetentionForOffenceList.InternalList.Where(o => o.Id == id).Select(o => (IStandardDetentionForOffence)o).First();
        }

        void ICRUDService<StandardDetentionForOffence, IStandardDetentionForOffence>.Add(IStandardDetentionForOffence entity)
        {
            this.Repository.StandardDetentionForOffenceList.InternalList.Add(entity);
        }

        public void AddList(List<IStandardDetentionForOffence> entityList)
        {
            this.Repository.StandardDetentionForOffenceList.InternalList.AddRange(entityList);
        }
    }
}
