using System;
using System.Collections.Generic;
using System.Linq;
using DetentionCalculator.Core.Entities;

namespace DetentionCalculator.Core
{
    public interface IRepository
    {
        List<IStandardDetentionForOffence> StandardDetentionForOffenceList { get;}
        List<IOffence> OffenceList { get; }
        List<IStudent> StudentList { get; }
        List<IFaculty> FacultyList { get; }
        List<IStudentOffence> StudentOffenceList { get; }
        List<IStudentDetention> StudentDetentionList { get; }

        List<ICalculateDetentionRequest> CalculateDetentionRequestList { get; }

        bool SaveChanges();
        bool DiscardChanges();
    }
    public class LocalRepository : IRepository
    {
        private const string LOCAL_FILE_NAME = "DetentionCalculatorRepository.xml";
        public LocalRepository()
        {
            //HydrateRepository
            //If local file doesn't exists, create shipment data and create the local file.
            //Load load from local file.
        }

        public List<ICalculateDetentionRequest> CalculateDetentionRequestList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IFaculty> FacultyList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IOffence> OffenceList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IStandardDetentionForOffence> StandardDetentionForOffenceList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IStudentDetention> StudentDetentionList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IStudent> StudentList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IStudentOffence> StudentOffenceList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool DiscardChanges()
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
