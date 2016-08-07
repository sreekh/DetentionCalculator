using DetentionCalculator.Core.Databases;
using DetentionCalculator.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetentionCalculator.Core.Services
{
    public interface ICRUDService<T>
        where T : IDEntity
    {
        List<T> Get();
        T Get(Guid id);
        void Add(T entity);
        void AddList(List<T> entityList);
    }
    public class OffenceService : ICRUDService<IOffence>
    {
        private IRepository Repository;
        public OffenceService(IRepository repository)
        {
            this.Repository = repository;
        }
        List<IOffence> ICRUDService<IOffence>.Get()
        {
            return this.Repository.OffenceList.InternalList.Select(o => (IOffence)o).ToList();
        }

        IOffence ICRUDService<IOffence>.Get(Guid id)
        {
            return this.Repository.OffenceList.InternalList.Where(o => o.Id == id).Select(o => (IOffence)o).First();
        }

        void ICRUDService<IOffence>.Add(IOffence entity)
        {
            this.Repository.OffenceList.InternalList.Add(entity);
        }

        public void AddList(List<IOffence> entityList)
        {
            this.Repository.OffenceList.InternalList.AddRange(entityList);
        }
    }
    public class StandardDetentionForOffenceService : ICRUDService<IStandardDetentionForOffence>
    {
        private IRepository Repository;
        public StandardDetentionForOffenceService(IRepository repository)
        {
            this.Repository = repository;
        }
        List<IStandardDetentionForOffence> ICRUDService<IStandardDetentionForOffence>.Get()
        {
            return this.Repository.StandardDetentionForOffenceList.InternalList.Select(o => (IStandardDetentionForOffence)o).ToList();
        }

        IStandardDetentionForOffence ICRUDService<IStandardDetentionForOffence>.Get(Guid id)
        {
            return this.Repository.StandardDetentionForOffenceList.InternalList.Where(o => o.Id == id).Select(o => (IStandardDetentionForOffence)o).First();
        }

        void ICRUDService<IStandardDetentionForOffence>.Add(IStandardDetentionForOffence entity)
        {
            this.Repository.StandardDetentionForOffenceList.InternalList.Add(entity);
        }

        public void AddList(List<IStandardDetentionForOffence> entityList)
        {
            this.Repository.StandardDetentionForOffenceList.InternalList.AddRange(entityList);
        }
    }
}
