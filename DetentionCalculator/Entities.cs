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
        where T : DEntity, new()
        where I : IDEntity
    {
        List<I> InternalList { get; set; }
    }
    public class DEntityList : List<DEntity>, IDEntityList<DEntity,IDEntity>
    {
        private List<IDEntity> internalList;
        public DEntityList() { }
        public DEntityList(List<IDEntity> internalList)  { this.internalList = internalList; }
        public DEntityList(IEnumerable<DEntity> internalList) : base(internalList){  }
        public List<IDEntity> InternalList
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
    }
    public class DEntityList<T,I> : IDEntityList<T,I>
        where T : DEntity, new()
        where I : IDEntity
    {
        private List<I> internalList;
        public DEntityList() { this.internalList = new List<I>(); }
        public DEntityList(List<I> internalList) { this.internalList = internalList; }
        public DEntityList(IEnumerable<I> internalList) { this.internalList = new List<I>(internalList); }
        public List<I> InternalList
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
    }

    public interface IPerson : IDEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DateOfBirth { get; set; }
    }
    public interface IStudent : IDEntity
    {
        IPerson PersonalDetails { get; set; }
        string RollNumber { get; set; }
        IClassRoom ClassRoom { get; set; }
        DateTime AcedemicYear { get; set; }
        bool IsActive { get; set; }
    }
    public interface IFaculty : IDEntity
    {
        IPerson PersonalDetails { get; set; }
        string FacultyId { get; set; }
        FacultyType FacultyType { get; set; }
        DateTime DateOfJoining { get; set; }
        bool IsInService { get; set; }
    }
    public class Faculty : DEntity, IFaculty
    {
        public IPerson PersonalDetails { get; set; }
        public string FacultyId { get; set; }
        public FacultyType FacultyType { get; set; }
        public DateTime DateOfJoining { get; set; }
        public bool IsInService { get; set; }
    }
    public class FacultyList : DEntityList<Faculty, IFaculty>, IDEntityList<Faculty, IFaculty>
    {
        public FacultyList() { }
        public FacultyList(List<IFaculty> internalList) : base(internalList) { this.InternalList = internalList; }
        public FacultyList(IEnumerable<IFaculty> internalList) { this.InternalList = new List<IFaculty>(internalList); }
    }
    public interface IClassRoom : IDEntity
    {
        IStandard Standard { get; set; }
        string Section { get; set; }
    }
    public class ClassRoom : DEntity, IClassRoom
    {
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
        public IPerson PersonalDetails { get; set; }
        public string RollNumber { get; set; }
        public IClassRoom ClassRoom { get; set; }
        public DateTime AcedemicYear { get; set; }
        public bool IsActive { get; set; }
    }
    public class StudentList : DEntityList<Student, IStudent>, IDEntityList<Student, IStudent>
    {
        public StudentList() : base() { }
        public StudentList(List<IStudent> internalList) : base(internalList) { this.InternalList = internalList; }
        public StudentList(IEnumerable<IStudent> internalList) { this.InternalList = new List<IStudent>(internalList); }
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
        public OffenceList(List<IOffence> internalList) : base(internalList) { this.InternalList = internalList; }
        public OffenceList(IEnumerable<IOffence> internalList) { this.InternalList = new List<IOffence>(internalList); }
    }
    public interface IDetentionForOffence
    {
        IOffence Offence { get; set; }
        double DetentionInHours { get; set; }
    }
    public class DetentionForOffence : IDetentionForOffence
    {
        public IOffence Offence { get; set; }
        public double DetentionInHours { get; set; }
    }
    
    public interface IStandardDetentionForOffence : IDEntity, IDetentionForOffence
    {
    }
    public class StandardDetentionForOffence : DEntity, IStandardDetentionForOffence
    {
        public IOffence Offence { get; set; }
        public double DetentionInHours { get; set; }
    }
    public class StandardDetentionForOffenceList : DEntityList<StandardDetentionForOffence, IStandardDetentionForOffence>, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence>
    {
        public StandardDetentionForOffenceList() :base() { }
        public StandardDetentionForOffenceList(List<IStandardDetentionForOffence> internalList) : base(internalList) { this.InternalList = internalList; }
        public StandardDetentionForOffenceList(IEnumerable<IStandardDetentionForOffence> internalList) { this.InternalList = new List<IStandardDetentionForOffence>(internalList); }
    }
    public interface IStudentOffence : IDEntity
    {
        IStudent Student { get; set; }
        IOffence Offence { get; set; }
        DateTime OffenceTime { get; set; }
        IFaculty ReportingFaculty { get; set; }
    }
    public class StudentOffence : DEntity, IStudentOffence
    {
        public IStudent Student { get; set; }
        public IOffence Offence { get; set; }
        public DateTime OffenceTime { get; set; }
        public IFaculty ReportingFaculty { get; set; }
    }
    public class StudentOffenceList : DEntityList<StudentOffence, IStudentOffence>, IDEntityList<StudentOffence, IStudentOffence>
    {
        public StudentOffenceList() { }
        public StudentOffenceList(List<IStudentOffence> internalList) : base(internalList) { this.InternalList = internalList; }
        public StudentOffenceList(IEnumerable<IStudentOffence> internalList) { this.InternalList = new List<IStudentOffence>(internalList); }
    }
    public interface IStudentDetention : IDEntity
    {
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
        public StudentDetentionList(List<IStudentDetention> internalList) : base(internalList) { this.InternalList = internalList; }
        public StudentDetentionList(IEnumerable<IStudentDetention> internalList) { this.InternalList = new List<IStudentDetention>(internalList); }
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
        IStudent Student { get; set; }
        IRuleCalculationMode RuleCalculationMode { get; set; }
        DateTime DetentionStartTime { get; set; }
        IFaculty RequestingFaculty { get; set; }
    }
    public class CalculateDetentionRequest : DEntity, ICalculateDetentionRequest
    {
        public IStudent Student { get; set; }
        public IRuleCalculationMode RuleCalculationMode { get; set; }
        public DateTime DetentionStartTime { get; set; }
        public IFaculty RequestingFaculty { get; set; }
    }
    public class CalculateDetentionRequestList : DEntityList<CalculateDetentionRequest, ICalculateDetentionRequest>, IDEntityList<CalculateDetentionRequest, ICalculateDetentionRequest>
    {
        public CalculateDetentionRequestList() : base() { }
        public CalculateDetentionRequestList(List<ICalculateDetentionRequest> internalList) : base(internalList) { this.InternalList = internalList; }
        public CalculateDetentionRequestList(IEnumerable<ICalculateDetentionRequest> internalList) { this.InternalList = new List<ICalculateDetentionRequest>(internalList); }
    }
    public interface ICalculateDetentionResponse
    {
        ICalculateDetentionRequest CalculateDetentionRequest { get; set; }
        StudentDetentionList StudentDetentionList { get; set; }
        double DetentionPeriodInHours { get; }
    }
    public class CalculateDetentionResponse : ICalculateDetentionResponse
    {
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
