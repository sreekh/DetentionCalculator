using DetentionCalculator.Core.Services;
using NUnit.Framework;
using DetentionCalculator.Core.Entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DetentionCalculator.TestingConsole
{
    [TestFixture]
    public class DetentionCalculatorTests
    {
        private static StandardKernel kernel;
        private static IStudentCRUDService studentService;
        private static IFacultyCRUDService facultyService;
        private static IDetentionCalculatorService detentionCalculatorService;
        private static ICalculateDetentionRequest calculateRequest;
        [SetUp]
        public void SetUp()
        {
            kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            studentService = kernel.Get<IStudentCRUDService>();
            facultyService = kernel.Get<IFacultyCRUDService>();
            calculateRequest = kernel.Get<ICalculateDetentionRequest>();
            calculateRequest.RuleCalculationMode = kernel.Get<IRuleCalculationMode>();
            calculateRequest.RequestingFaculty = facultyService.Get().InternalList.First();
            detentionCalculatorService = kernel.Get<IDetentionCalculatorService>();
        }
        [Test]
        private void NoDetentionTest()
        {
            calculateRequest.RuleCalculationMode.CalculationType =  RuleCalculationModeType.Concurrent;
            calculateRequest.Student = studentService.Get().InternalList.Where(x => x.RollNumber == "001").First();
            calculateRequest.DetentionStartTime = DateTime.Now;
            var response = detentionCalculatorService.CalculateDetention(calculateRequest);
            Assert.IsNull(response);

            calculateRequest.RuleCalculationMode.CalculationType =  RuleCalculationModeType.Consecutive;
            response = detentionCalculatorService.CalculateDetention(calculateRequest);
            Assert.IsNull(response);
        }
        [Test]
        private void TestGoodStudentDetentionScenario()
        {

        }
        [Test]
        private void TestBadStudentDetentionScenario()
        {

        }
        [Test]
        private void TestDetentionLimitExceedingScenario()
        {

        }
    }
}
