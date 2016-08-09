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
            calculateRequest.DetentionStartTime = DateTime.Now;
            var response = detentionCalculatorService.CalculateDetention(calculateRequest);

            Console.WriteLine("Hello!");
            Console.ReadLine();
        }
        private static void InitializeKernel()
        {
            kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            studentService = kernel.Get<IStudentCRUDService>();
            facultyService = kernel.Get<IFacultyCRUDService>();
            calculateRequest = kernel.Get<ICalculateDetentionRequest>();
            calculateRequest.RuleCalculationMode = kernel.Get<IRuleCalculationMode>();
            calculateRequest.RequestingFaculty = facultyService.Get().InternalList.First();
            calculateRequest.Student = studentService.Get().InternalList.First();
            detentionCalculatorService = kernel.Get<IDetentionCalculatorService>();
        }
    }
}
