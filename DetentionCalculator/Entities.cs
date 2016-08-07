using System;
using System.Collections.Generic;

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
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
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
    public interface IClassRoom : IDEntity
    {
        IStandard Standard { get; set; }
        string Section { get; set; }
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
    public interface IDetentionForOffence
    {
        IOffence Offence { get; set; }
        float DetentionInHours { get; set; }
    }
    public class DetentionForOffence : IDetentionForOffence
    {
        public IOffence Offence { get; set; }
        public float DetentionInHours { get; set; }
    }
    public interface IStandardDetentionForOffence : IDEntity, IDetentionForOffence
    {
    }
    public class StandardDetentionForOffence : DEntity, IStandardDetentionForOffence
    {
        public IOffence Offence { get; set; }
        public float DetentionInHours { get; set; }
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
    public interface IStudentDetention : IDEntity
    {
        IStudentOffence StudentOffence { get; set; }
        float DetentionInHours { get; set; }
        DateTime DetentionStartTime { get; set; }
        DateTime DetentionEndTime { get; set; }
        DateTime? DetentionActualStartTime { get; set; }
        DateTime? DetentionActualEndTime { get; set; }
        bool DetentionServed { get; }
    }
    public class StudentDetention : DEntity, IStudentDetention
    {
        public IStudentOffence StudentOffence { get; set; }
        public float DetentionInHours { get; set; }
        public DateTime DetentionStartTime { get; set; }
        public DateTime DetentionEndTime { get; set; }
        public DateTime? DetentionActualStartTime { get; set; }
        public DateTime? DetentionActualEndTime { get; set; }
        public bool DetentionServed { get; }
    }
    public interface IRuleCalculationMode : IDEntity
    {
        RuleCalculationModeType CalculationType { get; set; }
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
    public interface ICalculateDetentionResponse
    {
        ICalculateDetentionRequest CalculateDetentionRequest { get; set; }
        List<IStudentDetention> DetentionList { get; set; }
    }
    public class CalculateDetentionResponse : ICalculateDetentionResponse
    {
        public ICalculateDetentionRequest CalculateDetentionRequest { get; set; }
        public List<IStudentDetention> DetentionList { get; set; }
    }
}
