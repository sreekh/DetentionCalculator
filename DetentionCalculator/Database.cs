using System;
using System.Collections.Generic;
using System.Linq;
using DetentionCalculator.Core.Entities;
using System.IO;
using Newtonsoft.Json;

namespace DetentionCalculator.Core.Databases
{
    public interface IRepository
    {
        StandardDetentionForOffenceList StandardDetentionForOffenceList { get;}

        OffenceList OffenceList { get; }

        StudentList StudentList { get; }

        FacultyList FacultyList { get; }

        StudentOffenceList StudentOffenceList { get; }

        StudentDetentionList StudentDetentionList { get; }

        CalculateDetentionRequestList CalculateDetentionRequestList { get; }

        void SaveChanges();

        void DiscardChanges();
    }

    public class LocalRepository : IRepository
    {
        private const string LOCAL_FILE_DIRECTORY_PATH = @"C:\Temp\DetentionCalculator";
        private const string LOCAL_FILE_NAME = "DetentionCalculatorRepository.json";
        private SeedDataProvider seedDataProvider;

        public LocalRepository()
        {
            HydrateRepository();
        }

        private void HydrateRepository()
        {
            //If local file doesn't exists, create shipment data and create the local file.
            //Load load from local file.
            if(!File.Exists(Path.Combine(LOCAL_FILE_DIRECTORY_PATH, LOCAL_FILE_NAME)))
            {
                if (!Directory.Exists(LOCAL_FILE_DIRECTORY_PATH))
                    Directory.CreateDirectory(LOCAL_FILE_DIRECTORY_PATH);
                this.seedDataProvider = new SeedDataProvider();
                this.seedDataProvider.SetSeedData();
                SaveChanges();
            }
            DiscardChanges();
        }

        public CalculateDetentionRequestList CalculateDetentionRequestList
        {
            get
            {
                return this.seedDataProvider.CalculateDetentionRequestList;
            }
        }

        public FacultyList FacultyList
        {
            get
            {
                return this.seedDataProvider.FacultyList;
            }
        }

        public OffenceList OffenceList
        {
            get
            {
                return this.seedDataProvider.OffenceList;
            }
        }

        public StandardDetentionForOffenceList StandardDetentionForOffenceList
        {
            get
            {
                return this.seedDataProvider.StandardDetentionForOffenceList;
            }
        }

        public StudentDetentionList StudentDetentionList
        {
            get
            {
                return this.seedDataProvider.StudentDetentionList;
            }
        }

        public StudentList StudentList
        {
            get
            {
                return this.seedDataProvider.StudentList;
            }
        }

        public StudentOffenceList StudentOffenceList
        {
            get
            {
                return this.seedDataProvider.StudentOffenceList;
            }
        }

        public void DiscardChanges()
        {
            if (File.Exists(Path.Combine(LOCAL_FILE_DIRECTORY_PATH, LOCAL_FILE_NAME)))
                this.seedDataProvider = JsonConvert.DeserializeObject<SeedDataProvider>(File.ReadAllText(Path.Combine(LOCAL_FILE_DIRECTORY_PATH, LOCAL_FILE_NAME)));
            else
                throw new FileNotFoundException(string.Format("Repository not found at {0}", Path.Combine(LOCAL_FILE_DIRECTORY_PATH, LOCAL_FILE_NAME)));
        }

        public void SaveChanges()
        {
            if (this.seedDataProvider != null)
            {
                string serializedRepository = JsonConvert.SerializeObject(this.seedDataProvider, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                File.WriteAllText(Path.Combine(LOCAL_FILE_DIRECTORY_PATH, LOCAL_FILE_NAME), serializedRepository);
            }
            else
                throw new InvalidDataException("Seed data not initialized");
        }
    }

    internal class SeedDataProvider : IRepository
    {
        internal void SetSeedData()
        {
            this.StandardDetentionForOffenceList = new StandardDetentionForOffenceList();
            this.StandardDetentionForOffenceList.InternalList.Add(new StandardDetentionForOffence { DetentionInHours = 1, Offence = new Offence { Code = "Homework not done", Description = "Homework not done." } });
            this.StandardDetentionForOffenceList.InternalList.Add(new StandardDetentionForOffence { DetentionInHours = 2, Offence = new Offence { Code = "Stealing", Description = "Stealing." } });
            this.StandardDetentionForOffenceList.InternalList.Add(new StandardDetentionForOffence { DetentionInHours = 0.5, Offence = new Offence { Code = "Fighting", Description = "Fighting." } });
            this.StandardDetentionForOffenceList.InternalList.Add(new StandardDetentionForOffence { DetentionInHours = 1, Offence = new Offence { Code = "Untidyness", Description = "Untidyness." } });
            this.StandardDetentionForOffenceList.InternalList.Add(new StandardDetentionForOffence { DetentionInHours = 1.5, Offence = new Offence { Code = "Lying", Description = "Lying." } });
            this.StandardDetentionForOffenceList.InternalList.Add(new StandardDetentionForOffence { DetentionInHours = 1, Offence = new Offence { Code = "School Property Damage", Description = "Damaging school property." } });

            this.OffenceList = new OffenceList(this.StandardDetentionForOffenceList.InternalList.Select(s => s.Offence));

            this.FacultyList = new FacultyList();
            this.FacultyList.InternalList.Add(new Faculty
            {
                PersonalDetails = new Person
                {
                    FirstName = "George",
                    LastName = "Wilson",
                    DateOfBirth = new DateTime(1950, 01, 01)
                },
                DateOfJoining = new DateTime(2010, 01, 01),
                FacultyId = "E00100",
                FacultyType = FacultyType.Administration,
                IsInService = true
            });
            this.StudentList = new StudentList();
            this.StudentList.InternalList.Add(new Student
            {
                PersonalDetails = new Person { FirstName = "Joey", LastName = "McDonald", DateOfBirth = new DateTime(2010, 03, 01) },
                AcedemicYear = new DateTime(2016, 01, 01),
                IsActive = true,
                ClassRoom = new ClassRoom { Section = "A", Standard = new Standard { Type = StandardType.LowerKindergarten } },
                RollNumber = "001"
            });
            this.StudentList.InternalList.Add(new Student
            {
                PersonalDetails = new Person { FirstName = "Margaret", LastName = "Wade", DateOfBirth = new DateTime(2010, 02, 01) },
                AcedemicYear = new DateTime(2016, 01, 01),
                IsActive = true,
                ClassRoom = new ClassRoom { Section = "A", Standard = new Standard { Type = StandardType.LowerKindergarten } },
                RollNumber = "002"
            });
            this.StudentList.InternalList.Add(new Student
            {
                PersonalDetails = new Person { FirstName = "Dennis", LastName = "Mitchel", DateOfBirth = new DateTime(2010, 01, 01) },
                AcedemicYear = new DateTime(2016, 01, 01),
                IsActive = true,
                ClassRoom = new ClassRoom { Section = "A", Standard = new Standard { Type = StandardType.LowerKindergarten } },
                RollNumber = "003"
            });
            this.StudentList.InternalList.Add(new Student
            {
                PersonalDetails = new Person { FirstName = "Cowboy", LastName = "Bob", DateOfBirth = new DateTime(2010, 05, 01) },
                AcedemicYear = new DateTime(2016, 01, 01),
                IsActive = true,
                ClassRoom = new ClassRoom { Section = "A", Standard = new Standard { Type = StandardType.LowerKindergarten } },
                RollNumber = "004"
            });
            this.StudentDetentionList = new StudentDetentionList();
            this.StudentOffenceList = new StudentOffenceList();
            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(1), Offence = this.OffenceList.InternalList.ElementAt(4), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();

            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(2), Offence = this.OffenceList.InternalList.ElementAt(0), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();
            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(2), Offence = this.OffenceList.InternalList.ElementAt(1), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();

            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(3), Offence = this.OffenceList.InternalList.ElementAt(1), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();
            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(3), Offence = this.OffenceList.InternalList.ElementAt(1), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();
            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(3), Offence = this.OffenceList.InternalList.ElementAt(1), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();
            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(3), Offence = this.OffenceList.InternalList.ElementAt(1), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();
            this.StudentOffenceList.Add(new StudentOffence { Student = this.StudentList.InternalList.ElementAt(3), Offence = this.OffenceList.InternalList.ElementAt(1), ReportingFaculty = this.FacultyList.InternalList.ElementAt(0) });            this.CalculateDetentionRequestList = new CalculateDetentionRequestList();
        }

        public StandardDetentionForOffenceList StandardDetentionForOffenceList { get; set; }
        
        public OffenceList OffenceList { get; set; }

        public StudentList StudentList { get; set; }

        public FacultyList FacultyList { get; set; }

        public StudentOffenceList StudentOffenceList { get; set; }

        public StudentDetentionList StudentDetentionList { get; set; }

        public CalculateDetentionRequestList CalculateDetentionRequestList { get; set; }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void DiscardChanges()
        {
            throw new NotImplementedException();
        }
    }
}
