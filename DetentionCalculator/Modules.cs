using DetentionCalculator.Core.Databases;
using DetentionCalculator.Core.Entities;
using DetentionCalculator.Core.Processors;
using Ninject.Modules;

namespace DetentionCalculator.Core
{
    public class Modules : NinjectModule
    {
        public override void Load()
        {
            BindEntities();
            BindRepositories();
            BindProcessors();
        }
        private void BindEntities()
        {
            Bind<IDEntity>().To<DEntity>();
            Bind<IStudent>().To<Student>();
            Bind<IFaculty>().To<Faculty>();
            Bind<IStandard>().To<Standard>();
            Bind<IClassRoom>().To<ClassRoom>();
            Bind<IPerson>().To<Person>();
            Bind<IOffence>().To<Offence>();
            Bind<IDetentionForOffence>().To<DetentionForOffence>();
            Bind<IStandardDetentionForOffence>().To<StandardDetentionForOffence>();
            Bind<IStudentOffence>().To<StudentOffence>();
            Bind<IStudentDetention>().To<StudentDetention>();
            Bind<IRuleCalculationMode>().To<RuleCalculationMode>();
            Bind<ICalculateDetentionRequest>().To<CalculateDetentionRequest>();
            Bind<ICalculateDetentionResponse>().To<CalculateDetentionResponse>();
        }
        private void BindRepositories()
        {
            Bind<IRepository>().To<LocalRepository>().InSingletonScope();
        }
        private void BindProcessors()
        {
            Bind<IDetentionCalculator>().To<DetentionCalculator.Core.Processors.DetentionCalculator>();
        }
    }
}
