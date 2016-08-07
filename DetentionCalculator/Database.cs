using System;
using System.Collections.Generic;
using System.Linq;
using DetentionCalculator.Core.Entities;
using System.IO;
using Newtonsoft.Json;

namespace DetentionCalculator.Core
{
    public interface IRepository
    {
        List<StandardDetentionForOffence> StandardDetentionForOffenceList { get;}

        List<Offence> OffenceList { get; }

        List<Student> StudentList { get; }

        List<Faculty> FacultyList { get; }

        List<StudentOffence> StudentOffenceList { get; }

        List<StudentDetention> StudentDetentionList { get; }

        List<CalculateDetentionRequest> CalculateDetentionRequestList { get; }

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

        public List<CalculateDetentionRequest> CalculateDetentionRequestList
        {
            get
            {
                return this.seedDataProvider.CalculateDetentionRequestList;
            }
        }

        public List<Faculty> FacultyList
        {
            get
            {
                return this.seedDataProvider.FacultyList;
            }
        }

        public List<Offence> OffenceList
        {
            get
            {
                return this.seedDataProvider.OffenceList;
            }
        }

        public List<StandardDetentionForOffence> StandardDetentionForOffenceList
        {
            get
            {
                return this.seedDataProvider.StandardDetentionForOffenceList;
            }
        }

        public List<StudentDetention> StudentDetentionList
        {
            get
            {
                return this.seedDataProvider.StudentDetentionList;
            }
        }

        public List<Student> StudentList
        {
            get
            {
                return this.seedDataProvider.StudentList;
            }
        }

        public List<StudentOffence> StudentOffenceList
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
                string serializedRepository = JsonConvert.SerializeObject(this.seedDataProvider);
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
            this.StandardDetentionForOffenceList = new List<StandardDetentionForOffence>();
            this.StandardDetentionForOffenceList.Add(new StandardDetentionForOffence { DetentionInHours = 1, Offence = new Offence { Code = "Homework not done", Description = "Homework not done." } });
            this.StandardDetentionForOffenceList.Add(new StandardDetentionForOffence { DetentionInHours = 2, Offence = new Offence { Code = "Stealing", Description = "Stealing." } });
            this.StandardDetentionForOffenceList.Add(new StandardDetentionForOffence { DetentionInHours = 0.5, Offence = new Offence { Code = "Fighting", Description = "Fighting." } });
            this.StandardDetentionForOffenceList.Add(new StandardDetentionForOffence { DetentionInHours = 1, Offence = new Offence { Code = "Untidyness", Description = "Untidyness." } });
            this.StandardDetentionForOffenceList.Add(new StandardDetentionForOffence { DetentionInHours = 1.5, Offence = new Offence { Code = "Lying", Description = "Lying." } });
            this.StandardDetentionForOffenceList.Add(new StandardDetentionForOffence { DetentionInHours = 1, Offence = new Offence { Code = "School Property Damage", Description = "Damaging school property." } });

            this.OffenceList = this.StandardDetentionForOffenceList.Select(s => (Offence)s.Offence).ToList();
        }

        public List<StandardDetentionForOffence> StandardDetentionForOffenceList { get; set; }
        
        public List<Offence> OffenceList { get; set; }

        public List<Student> StudentList { get; set; }

        public List<Faculty> FacultyList { get; set; }

        public List<StudentOffence> StudentOffenceList { get; set; }

        public List<StudentDetention> StudentDetentionList { get; set; }

        public List<CalculateDetentionRequest> CalculateDetentionRequestList { get; set; }

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
