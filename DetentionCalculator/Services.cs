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
        void Delete(I entity);
    }
    public abstract class BaseCRUDService<T, I> : ICRUDService<T, I>
        where T : DEntity, new()
        where I : IDEntity
    {
        protected IRepository Repository;
        public BaseCRUDService(IRepository repository)
        {
            this.Repository = repository;
        }

        public void Add(I entity)
        {
            ProcessAdd(entity);
            SaveChanges();
        }
        protected abstract void ProcessAdd(I entity);

        public void AddList(List<I> entityList)
        {
            ProcessAddList(entityList);
            SaveChanges();
        }
        protected abstract void ProcessAddList(List<I> entityList);

        public void Delete(I entity)
        {
            ProcessDelete(entity);
            SaveChanges();
        }
        protected abstract void ProcessDelete(I entity);

        public IDEntityList<T, I> Get()
        {
            return ProcessGet();
        }
        protected abstract IDEntityList<T, I> ProcessGet();

        public I Get(Guid id)
        {
            return ProcessGet(id);
        }
        protected abstract I ProcessGet(Guid id);

        protected void SaveChanges()
        {
            this.Repository.SaveChanges();
        }
    }
    public interface IOffenceCRUDService : ICRUDService<Offence, IOffence> { }
    public class OffenceService : BaseCRUDService<Offence, IOffence>, IOffenceCRUDService
    {
        public OffenceService(IRepository repository)
            :base(repository)
        {
        }

        protected override IDEntityList<Offence, IOffence> ProcessGet()
        {
            return this.Repository.OffenceList;
        }

        protected override IOffence ProcessGet(Guid id)
        {
            return this.Repository.OffenceList.InternalList.Where(o => o.Id == id).Select(o => (IOffence)o).First();
        }

        protected override void ProcessAdd(IOffence entity)
        {
            this.Repository.OffenceList.InternalList.Add(entity);
        }

        protected override void ProcessDelete(IOffence entity)
        {
            this.Repository.OffenceList.InternalList.Remove(entity);
        }

        protected override void ProcessAddList(List<IOffence> entityList)
        {
            this.Repository.OffenceList.InternalList.AddRange(entityList);
        }
    }
    public interface IStandardDetentionForOffenceCRUDService : ICRUDService<StandardDetentionForOffence, IStandardDetentionForOffence> { }
    public class StandardDetentionForOffenceService : BaseCRUDService<StandardDetentionForOffence, IStandardDetentionForOffence>, IStandardDetentionForOffenceCRUDService
    {
        public StandardDetentionForOffenceService(IRepository repository)
            :base(repository)
        {
            
        }
        protected override IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> ProcessGet()
        {
            return this.Repository.StandardDetentionForOffenceList;
        }

        protected override IStandardDetentionForOffence ProcessGet(Guid id)
        {
            return this.Repository.StandardDetentionForOffenceList.InternalList.Where(o => o.Id == id).Select(o => (IStandardDetentionForOffence)o).First();
        }

        protected override void ProcessDelete(IStandardDetentionForOffence entity)
        {
            this.Repository.StandardDetentionForOffenceList.InternalList.Remove(entity);
        }

        protected override void ProcessAdd(IStandardDetentionForOffence entity)
        {
            this.Repository.StandardDetentionForOffenceList.InternalList.Add(entity);
        }

        protected override void ProcessAddList(List<IStandardDetentionForOffence> entityList)
        {
            this.Repository.StandardDetentionForOffenceList.InternalList.AddRange(entityList);
        }
    }
    public interface IStudentCRUDService : ICRUDService<Student, IStudent> { }
    public class StudentService : BaseCRUDService<Student, IStudent>, IStudentCRUDService
    {
        public StudentService(IRepository repository)
            : base(repository)
        {
        }
        protected override void ProcessAdd(IStudent entity)
        {
            this.Repository.StudentList.InternalList.Add(entity);
        }

        protected override void ProcessDelete(IStudent entity)
        {
            this.Repository.StudentList.InternalList.Remove(entity);
        }

        protected override void ProcessAddList(List<IStudent> entityList)
        {
            this.Repository.StudentList.InternalList.AddRange(entityList);
        }

        protected override IDEntityList<Student, IStudent> ProcessGet()
        {
            return this.Repository.StudentList;
        }

        protected override IStudent ProcessGet(Guid id)
        {
            return this.Repository.StudentList.InternalList.Where(o => o.Id == id).Select(o => (IStudent)o).First();
        }
    }
    public interface IFacultyCRUDService : ICRUDService<Faculty, IFaculty> { }
    public class FacultyService : BaseCRUDService<Faculty, IFaculty>, IFacultyCRUDService
    {
        public FacultyService(IRepository repository)
            : base(repository)
        {
        }
        protected override void ProcessAdd(IFaculty entity)
        {
            this.Repository.FacultyList.InternalList.Add(entity);
        }

        protected override void ProcessDelete(IFaculty entity)
        {
            this.Repository.FacultyList.InternalList.Remove(entity);
        }

        protected override void ProcessAddList(List<IFaculty> entityList)
        {
            this.Repository.FacultyList.InternalList.AddRange(entityList);
        }

        protected override IDEntityList<Faculty, IFaculty> ProcessGet()
        {
            return this.Repository.FacultyList;
        }

        protected override IFaculty ProcessGet(Guid id)
        {
            return this.Repository.FacultyList.InternalList.Where(o => o.Id == id).Select(o => (IFaculty)o).First();
        }
    }
    public interface IStudentOffenceCRUDService : ICRUDService<StudentOffence, IStudentOffence>
    {
        IDEntityList<StudentOffence, IStudentOffence> Get(IStudent student);
    }
    public class StudentOffenceService : BaseCRUDService<StudentOffence, IStudentOffence>, IStudentOffenceCRUDService
    {
        public StudentOffenceService(IRepository repository)
            : base(repository)
        {
        }

        protected override void ProcessAdd(IStudentOffence entity)
        {
            this.Repository.StudentOffenceList.InternalList.Add(entity);
        }

        protected override void ProcessDelete(IStudentOffence entity)
        {
            this.Repository.StudentOffenceList.InternalList.Remove(entity);
        }

        protected override void ProcessAddList(List<IStudentOffence> entityList)
        {
            this.Repository.StudentOffenceList.InternalList.AddRange(entityList);
        }

        protected override IDEntityList<StudentOffence, IStudentOffence> ProcessGet()
        {
            return this.Repository.StudentOffenceList;
        }

        public IDEntityList<StudentOffence, IStudentOffence> Get(IStudent student)
        {
            if (student == null)
                throw new ArgumentNullException("student");

            return new StudentOffenceList(this.Repository.StudentOffenceList.InternalList.Where(so => so.Student.Id == student.Id));
        }

        protected override IStudentOffence ProcessGet(Guid id)
        {
            return this.Repository.StudentOffenceList.InternalList.Where(o => o.Id == id).Select(o => (IStudentOffence)o).First();
        }
    }
    public interface IStudentDetentionCRUDService : ICRUDService<StudentDetention, IStudentDetention> { }
    public class StudentDetentionService : BaseCRUDService<StudentDetention, IStudentDetention>, IStudentDetentionCRUDService
    {
        public StudentDetentionService(IRepository repository)
            : base(repository)
        {
        }
        protected override void ProcessAdd(IStudentDetention entity)
        {
            this.Repository.StudentDetentionList.InternalList.Add(entity);
        }

        protected override void ProcessDelete(IStudentDetention entity)
        {
            this.Repository.StudentDetentionList.InternalList.Remove(entity);
        }

        protected override void ProcessAddList(List<IStudentDetention> entityList)
        {
            this.Repository.StudentDetentionList.InternalList.AddRange(entityList);
        }

        protected override IDEntityList<StudentDetention, IStudentDetention> ProcessGet()
        {
            return this.Repository.StudentDetentionList;
        }

        protected override IStudentDetention ProcessGet(Guid id)
        {
            return this.Repository.StudentDetentionList.InternalList.Where(o => o.Id == id).Select(o => (IStudentDetention)o).First();
        }
    }
    public interface ICalculateDetentionRequestCRUDService : ICRUDService<CalculateDetentionRequest, ICalculateDetentionRequest> { }
    public class CalculateDetentionRequestService : BaseCRUDService<CalculateDetentionRequest, ICalculateDetentionRequest>, ICalculateDetentionRequestCRUDService
    {
        public CalculateDetentionRequestService(IRepository repository)
            : base(repository)
        {
        }
        protected override void ProcessAdd(ICalculateDetentionRequest entity)
        {
            this.Repository.CalculateDetentionRequestList.InternalList.Add(entity);
        }

        protected override void ProcessDelete(ICalculateDetentionRequest entity)
        {
            this.Repository.CalculateDetentionRequestList.InternalList.Remove(entity);
        }

        protected override void ProcessAddList(List<ICalculateDetentionRequest> entityList)
        {
            this.Repository.CalculateDetentionRequestList.InternalList.AddRange(entityList);
        }

        protected override IDEntityList<CalculateDetentionRequest, ICalculateDetentionRequest> ProcessGet()
        {
            return this.Repository.CalculateDetentionRequestList;
        }

        protected override ICalculateDetentionRequest ProcessGet(Guid id)
        {
            return this.Repository.CalculateDetentionRequestList.InternalList.Where(o => o.Id == id).Select(o => (ICalculateDetentionRequest)o).First();
        }
    }
}
