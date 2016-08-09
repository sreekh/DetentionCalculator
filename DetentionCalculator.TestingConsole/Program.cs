using DetentionCalculator.Core;
using DetentionCalculator.Core.Entities;
using DetentionCalculator.Core.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DetentionCalculator.TestingConsole
{
    public class Program
    {
        private static StandardKernel kernel;
        private static IStudentCRUDService studentService;
        private static IFacultyCRUDService facultyService;
        private static IDetentionCalculatorService detentionCalculatorService;
        private static ICalculateDetentionRequest calculateRequest;

        public static void Main(string[] args)
        {
            InitializeKernel();

            TestNoDetentionScenario(RuleCalculationModeType.Concurrent);
            TestNoDetentionScenario(RuleCalculationModeType.Consecutive);

            TestGoodStudentDetentionScenario(RuleCalculationModeType.Consecutive);
            TestGoodStudentDetentionScenario(RuleCalculationModeType.Concurrent);

            TestBadStudentDetentionScenario();
            TestDetentionLimitExceedingScenario();

            Console.WriteLine();
            Console.WriteLine("Tests completed! Hit any key to exit. . .");
            Console.ReadLine();
        }
        private static void InitializeKernel()
        {
            kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            studentService = kernel.Get<IStudentCRUDService>();
            facultyService = kernel.Get<IFacultyCRUDService>();

            detentionCalculatorService = kernel.Get<IDetentionCalculatorService>();
        }
        private static void TestNoDetentionScenario(RuleCalculationModeType calculationType)
        {
            calculateRequest = kernel.Get<ICalculateDetentionRequest>();
            calculateRequest.RequestingFaculty = facultyService.Get().InternalList.First();
            calculateRequest.RuleCalculationMode = kernel.Get<IRuleCalculationMode>();
            calculateRequest.RuleCalculationMode.CalculationType = calculationType;
            calculateRequest.Student = studentService.Get().InternalList.Where(x => x.RollNumber == "001").First(); // Has no offence registered
            calculateRequest.DetentionStartTime = DateTime.Now;
            var response = detentionCalculatorService.CalculateDetention(calculateRequest);

            if (response != null)
                throw new Exception("TestNoDetentionScenario failed. Student should not get detention since has no offence registered against him/her.");
            else
                Console.WriteLine("TestNoDetentionScenario Success.");
        }
        private static void TestGoodStudentDetentionScenario(RuleCalculationModeType calculationType)
        {
            calculateRequest = kernel.Get<ICalculateDetentionRequest>();
            calculateRequest.RequestingFaculty = facultyService.Get().InternalList.First();
            calculateRequest.RuleCalculationMode = kernel.Get<IRuleCalculationMode>();
            calculateRequest.RuleCalculationMode.CalculationType = calculationType;
            calculateRequest.Student = studentService.Get().InternalList.Where(x => x.RollNumber == "002").First(); // Has 1 offence (1.5 hours detention) registered
            calculateRequest.DetentionStartTime = DateTime.Now;
            var response = detentionCalculatorService.CalculateDetention(calculateRequest);
            if (response == null || response.DetentionPeriodInHours != 0.9 * 1.5)
                throw new Exception("TestGoodStudentDetentionScenario failed");
            else
                Console.WriteLine("TestGoodStudentDetentionScenario Success.");
        }
        private static void TestBadStudentDetentionScenario()
        {
            
            calculateRequest = kernel.Get<ICalculateDetentionRequest>();
            calculateRequest.RequestingFaculty = facultyService.Get().InternalList.First();
            calculateRequest.RuleCalculationMode = kernel.Get<IRuleCalculationMode>();
            calculateRequest.RuleCalculationMode.CalculationType = RuleCalculationModeType.Consecutive;
            calculateRequest.Student = studentService.Get().InternalList.Where(x => x.RollNumber == "003").First(); // Has 2 offences (1 hour and 2 hours detention) registered
            calculateRequest.DetentionStartTime = DateTime.Now;
            var response = detentionCalculatorService.CalculateDetention(calculateRequest);
            if (response == null || response.DetentionPeriodInHours != 1.1 * 3.0)
                throw new Exception("TestGoodStudentDetentionScenario (Consecutive) failed");
            else
                Console.WriteLine("TestGoodStudentDetentionScenario (Consecutive) Success.");
            calculateRequest.RuleCalculationMode.CalculationType = RuleCalculationModeType.Concurrent;
            response = detentionCalculatorService.CalculateDetention(calculateRequest);
            if (response == null || response.DetentionPeriodInHours != 1.1 * 2.0)
                throw new Exception("TestGoodStudentDetentionScenario (Concurrent) failed");
            else
                Console.WriteLine("TestGoodStudentDetentionScenario (Concurrent) Success.");
        }
        private static void TestDetentionLimitExceedingScenario()
        {
            try
            {
                calculateRequest = kernel.Get<ICalculateDetentionRequest>();
                calculateRequest.RequestingFaculty = facultyService.Get().InternalList.First();
                calculateRequest.RuleCalculationMode = kernel.Get<IRuleCalculationMode>();
                calculateRequest.RuleCalculationMode.CalculationType = RuleCalculationModeType.Consecutive;
                calculateRequest.Student = studentService.Get().InternalList.Where(x => x.RollNumber == "004").First(); // Has 5 offences (2 hour each detention) registered
                calculateRequest.DetentionStartTime = DateTime.Now;
                var response = detentionCalculatorService.CalculateDetention(calculateRequest);
                if (response == null || response.DetentionPeriodInHours > 8.0)
                    throw new Exception("TestDetentionLimitExceedingScenario (Consecutive) failed");
            }
            catch (DetentionExceedsDayLimitException ex)
            {
                 Console.WriteLine("TestGoodStudentDetentionScenario (Consecutive) Success.");
                Console.WriteLine("Actual detention hours received: " + ex.CalculateDetentionResponse.DetentionPeriodInHours);
            }
        }
    }
}
