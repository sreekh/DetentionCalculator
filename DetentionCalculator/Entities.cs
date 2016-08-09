using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DetentionCalculator.Core.Entities
{
    public interface IDEntity
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
    }
    public class DEntity : IDEntity
    {
        public DEntity()
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.UtcNow;
            this.Modified = DateTime.UtcNow;
            this.CreatedBy = "system";
            this.ModifiedBy = "system";
        }
        public DEntity(IDEntity entity)
        {
            if(entity != null)
            {
                this.Id = entity.Id;
                this.Created = entity.Created;
                this.Modified = entity.Modified;
                this.CreatedBy = !string.IsNullOrWhiteSpace(entity.CreatedBy) ? "system" : entity.CreatedBy;
                this.ModifiedBy = !string.IsNullOrWhiteSpace(entity.ModifiedBy) ? "system" : entity.ModifiedBy;
            }
            
        }
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    public interface IDEntityList<T,I>
        where T : DEntity, I, new()
        where I : IDEntity
    {
        List<T> InternalList { get; set; }
        void Remove(I entity);
        void Add(I entity);
        void AddRange(List<I> entityList);
    }
    public class DEntityList : List<DEntity>, IDEntityList<DEntity,IDEntity>
    {
        private List<DEntity> internalList;
        public DEntityList() { this.InternalList = new List<DEntity>(); }
        public DEntityList(List<IDEntity> internalList) : this() { if (internalList != null) internalList.ForEach(i => this.InternalList.Add((DEntity)i)); }
        public DEntityList(IEnumerable<IDEntity> internalList) : this(internalList.ToList()) { }
        public List<DEntity> InternalList
        {
            get
            {
                return this.internalList;
            }

            set
            {
                this.internalList = value;
            }
        }
        public void Remove(IDEntity entity)
        {
            if (this.InternalList != null)
                this.InternalList.Remove((DEntity)entity);
        }
        public void Add(IDEntity entity)
        {
            if (this.InternalList != null)
                this.InternalList.Add((DEntity)entity);
        }
        public void AddRange(List<IDEntity> entityList)
        {
            if (this.InternalList != null && entityList != null)
                entityList.ForEach(entity => this.InternalList.Add((DEntity)entity));
        }
    }
    public class DEntityList<T,I> : IDEntityList<T,I>
        where T : DEntity, I, new()
        where I : IDEntity
    {
        private List<T> internalList;
        public DEntityList() { this.internalList = new List<T>(); }
        public DEntityList(List<I> internalList):this() { if (internalList != null) internalList.ForEach(i => this.InternalList.Add((T)i)); }
        public DEntityList(IEnumerable<I> internalList):this(internalList.ToList()) { }

        public List<T> InternalList
        {
            get
            {
                return this.internalList;
            }

            set
            {
                this.internalList = value;
            }
        }
        public void Remove(I entity)
        {
            if (this.InternalList != null)
                this.InternalList.Remove((T)entity);
        }
        public void Add(I entity)
        {
            if (this.InternalList != null)
                this.InternalList.Add((T)entity);
        }
        public void AddRange(List<I> entityList)
        {
            if (this.InternalList != null && entityList != null)
                entityList.ForEach(entity => this.InternalList.Add((T)entity));
        }
    }

    public interface IPerson : IDEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DateOfBirth { get; set; }
    }
    public interface IStudent : IDEntity
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IPerson PersonalDetails { get; set; }
        string RollNumber { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IClassRoom ClassRoom { get; set; }
        DateTime AcedemicYear { get; set; }
        bool IsActive { get; set; }
    }
    public interface IFaculty : IDEntity
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IPerson PersonalDetails { get; set; }
        string FacultyId { get; set; }
        FacultyType FacultyType { get; set; }
        DateTime DateOfJoining { get; set; }
        bool IsInService { get; set; }
    }
    public class Faculty : DEntity, IFaculty
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IPerson PersonalDetails { get; set; }
        public string FacultyId { get; set; }
        public FacultyType FacultyType { get; set; }
        public DateTime DateOfJoining { get; set; }
        public bool IsInService { get; set; }
    }
    public class FacultyList : DEntityList<Faculty, IFaculty>, IDEntityList<Faculty, IFaculty>
    {
        public FacultyList() { }
        public FacultyList(List<IFaculty> internalList) : base(internalList) {  }
        public FacultyList(IEnumerable<IFaculty> internalList) :this(internalList.ToList()) { }
    }
    public interface IClassRoom : IDEntity
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IStandard Standard { get; set; }
        string Section { get; set; }
    }
    public class ClassRoom : DEntity, IClassRoom
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IStandard Standard { get; set; }
        public string Section { get; set; }
    }
    public enum FacultyType : short
    {
        Teaching = 0,
        NonTeaching = 1,
        Facilities = 2,
        Administration = 3,
    }
    public interface IStandard : IDEntity
    {
        StandardType Type { get; set; }
    }
    public class Standard : DEntity, IStandard
    {
        public StandardType Type { get; set; }
    }
    public enum StandardType : short
    {
        PreNursery = -4,
        Nursery = -3,
        LowerKindergarten = -2,
        UpperKindergarten = -1,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5,
        Sixth = 6,
        Seventh = 7,
        Eighth = 8,
        Ninth = 9,
        Tenth = 10,
        Eleventh = 11,
        Twelveth = 12,
    }
    public class Person : DEntity, IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
    public class Student : DEntity, IStudent
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IPerson PersonalDetails { get; set; }
        public string RollNumber { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IClassRoom ClassRoom { get; set; }
        public DateTime AcedemicYear { get; set; }
        public bool IsActive { get; set; }
    }
    public class StudentList : DEntityList<Student, IStudent>, IDEntityList<Student, IStudent>
    {
        public StudentList() : base() { }
        public StudentList(List<IStudent> internalList) : base(internalList) { }
        public StudentList(IEnumerable<IStudent> internalList) : this(internalList.ToList()) { }
    }
    public interface IOffence : IDEntity
    {
        string Code { get; set; }
        string Description { get; set; }
    }
    public class Offence : DEntity, IOffence
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class OffenceList : DEntityList<Offence, IOffence>, IDEntityList<Offence, IOffence>
    {
        public OffenceList() { }
        public OffenceList(List<IOffence> internalList) : base(internalList) { }
        public OffenceList(IEnumerable<IOffence> internalList) : this(internalList.ToList()) { }
    }
    public interface IDetentionForOffence
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IOffence Offence { get; set; }
        double DetentionInHours { get; set; }
    }
    public class DetentionForOffence : IDetentionForOffence
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IOffence Offence { get; set; }
        public double DetentionInHours { get; set; }
    }
    
    public interface IStandardDetentionForOffence : IDEntity, IDetentionForOffence
    {
    }
    public class StandardDetentionForOffence : DEntity, IStandardDetentionForOffence
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IOffence Offence { get; set; }
        public double DetentionInHours { get; set; }
    }
    public class StandardDetentionForOffenceList : DEntityList<StandardDetentionForOffence, IStandardDetentionForOffence>, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence>
    {
        public StandardDetentionForOffenceList() :base() { }
        public StandardDetentionForOffenceList(List<IStandardDetentionForOffence> internalList) : base(internalList) { }
        public StandardDetentionForOffenceList(IEnumerable<IStandardDetentionForOffence> internalList) : this(internalList.ToList()) { }
    }
    public interface IStudentOffence : IDEntity
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IStudent Student { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IOffence Offence { get; set; }
        DateTime OffenceTime { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IFaculty ReportingFaculty { get; set; }
    }
    public class StudentOffence : DEntity, IStudentOffence
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IStudent Student { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IOffence Offence { get; set; }
        public DateTime OffenceTime { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IFaculty ReportingFaculty { get; set; }
    }
    public class StudentOffenceList : DEntityList<StudentOffence, IStudentOffence>, IDEntityList<StudentOffence, IStudentOffence>
    {
        public StudentOffenceList() { }
        public StudentOffenceList(List<IStudentOffence> internalList) : base(internalList) { }
        public StudentOffenceList(IEnumerable<IStudentOffence> internalList) : this(internalList.ToList()) { }
    }
    public interface IStudentDetention : IDEntity
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IStudentOffence StudentOffence { get; set; }
        double DetentionInHours { get; set; }
        DateTime DetentionStartTime { get; set; }
        DateTime DetentionEndTime { get; set; }
        DateTime? DetentionActualStartTime { get; set; }
        DateTime? DetentionActualEndTime { get; set; }
        bool DetentionServed { get; }
    }
    public class StudentDetention : DEntity, IStudentDetention
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IStudentOffence StudentOffence { get; set; }
        public double DetentionInHours { get; set; }
        public DateTime DetentionStartTime { get; set; }
        public DateTime DetentionEndTime { get; set; }
        public DateTime? DetentionActualStartTime { get; set; }
        public DateTime? DetentionActualEndTime { get; set; }
        public bool DetentionServed { get; }
    }
    public class StudentDetentionList : DEntityList<StudentDetention, IStudentDetention>, IDEntityList<StudentDetention, IStudentDetention>
    {
        public StudentDetentionList() : base() { }
        public StudentDetentionList(List<IStudentDetention> internalList) : base(internalList) { }
        public StudentDetentionList(IEnumerable<IStudentDetention> internalList) : this(internalList.ToList()) { }
    }
    public interface IRuleCalculationMode : IDEntity
    {
        RuleCalculationModeType CalculationType { get; set; }
    }
    public class RuleCalculationMode : DEntity, IRuleCalculationMode
    {
        public RuleCalculationModeType CalculationType { get; set; }
    }
    public enum RuleCalculationModeType : short
    {
        Concurrent = 0,
        Consecutive = 1,
    }
    public interface ICalculateDetentionRequest : IDEntity
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IStudent Student { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IRuleCalculationMode RuleCalculationMode { get; set; }
        DateTime DetentionStartTime { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        IFaculty RequestingFaculty { get; set; }
    }
    public class CalculateDetentionRequest : DEntity, ICalculateDetentionRequest
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IStudent Student { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IRuleCalculationMode RuleCalculationMode { get; set; }
        public DateTime DetentionStartTime { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public IFaculty RequestingFaculty { get; set; }
    }
    public class CalculateDetentionRequestList : DEntityList<CalculateDetentionRequest, ICalculateDetentionRequest>, IDEntityList<CalculateDetentionRequest, ICalculateDetentionRequest>
    {
        public CalculateDetentionRequestList() : base() { }
        public CalculateDetentionRequestList(List<ICalculateDetentionRequest> internalList) : base(internalList) { }
        public CalculateDetentionRequestList(IEnumerable<ICalculateDetentionRequest> internalList) : this(internalList.ToList()) { }
    }
    public interface ICalculateDetentionResponse
    {
        ICalculateDetentionRequest CalculateDetentionRequest { get; set; }
        StudentDetentionList StudentDetentionList { get; set; }
        double DetentionPeriodInHours { get; }
    }
    public class CalculateDetentionResponse : ICalculateDetentionResponse
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public ICalculateDetentionRequest CalculateDetentionRequest { get; set; }
        public StudentDetentionList StudentDetentionList { get; set; }
        public double DetentionPeriodInHours
        {
            get
            {
                return this.StudentDetentionList != null && this.StudentDetentionList.InternalList.Count > 0 
                    ? this.StudentDetentionList.InternalList.Sum(sd => sd.DetentionInHours)
                    : 0;
            }
        }
    }
}
