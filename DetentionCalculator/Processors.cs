using System;
using System.Collections.Generic;
using System.Linq;
using DetentionCalculator.Core.Entities;
using DetentionCalculator.Core.Services;

namespace DetentionCalculator.Core.Processors
{
    public interface IDetentionCalculator
    {
        ICalculateDetentionResponse Calculate(ICalculateDetentionRequest request);
    }
    public class DetentionCalculator : IDetentionCalculator
    {
        private IStandardDetentionForOffenceCRUDService StandardDetentionForOffenceService;
        private IStudentOffenceCRUDService StudentOffenceCRUDService;
        private IDetentionForOffenceToCalculateDetentionResponseConverter DetentionForOffenceToCalculateDetentionResponseConverter;

        public DetentionCalculator(IStandardDetentionForOffenceCRUDService standardDetentionForOffenceCRUDService,
             IStudentOffenceCRUDService studentOffenceCRUDService, IDetentionForOffenceToCalculateDetentionResponseConverter detentionForOffenceToCalculateDetentionResponseConverter)
        {
            this.StandardDetentionForOffenceService = standardDetentionForOffenceCRUDService;
            this.StudentOffenceCRUDService = studentOffenceCRUDService;
            this.DetentionForOffenceToCalculateDetentionResponseConverter = detentionForOffenceToCalculateDetentionResponseConverter;
        }
        public ICalculateDetentionResponse Calculate(ICalculateDetentionRequest request)
        {
            List<DetentionForOffence> initialDetentionList = new List<DetentionForOffence>();
            if(request != null)
            {
                var detentionRules = GetDetentionRules();
                var standardDetentions = this.StandardDetentionForOffenceService.Get();
                var studentOffences = this.StudentOffenceCRUDService.Get(request.Student);
                if (detentionRules != null && detentionRules.Count() > 0 && studentOffences != null && studentOffences.InternalList.Count > 0)
                {
                    var offenceList = studentOffences.InternalList.Select(so => so.Offence);
                    detentionRules.ToList().ForEach(dr => {
                        var intermediateList = dr.GetDetention(offenceList, standardDetentions);
                        if(intermediateList != null)
                            initialDetentionList.AddRange(intermediateList);
                        }
                    );
                }

                var orchestrationStrategies = GetOrchestrationStrategies(request.RuleCalculationMode);
                if(orchestrationStrategies != null && orchestrationStrategies.Count() > 0)
                {
                    orchestrationStrategies.ToList().ForEach(os => initialDetentionList = os.FinalizeDetention(initialDetentionList));
                }
            }
                var response = this.DetentionForOffenceToCalculateDetentionResponseConverter.Convert(initialDetentionList, request);
            if (response != null && response.DetentionPeriodInHours > Constants.DetentionPeriodLimitForADay)
                throw new DetentionExceedsDayLimitException(response);
            return response;
        }

        private IEnumerable<IDetentionRule> GetDetentionRules()
        {
            var interfaceType = typeof(IDetentionRule);
            return AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(x => x.GetTypes())
              .Where(x => interfaceType.IsAssignableFrom(x)
                        && !x.IsInterface
                        && !x.IsAbstract)
              .Select(x => (IDetentionRule)Activator.CreateInstance(x));
        }
        private IEnumerable<IRuleOrchestrationStrategy> GetOrchestrationStrategies(IRuleCalculationMode ruleCalculationMode)
        {
            var interfaceType = typeof(IRuleOrchestrationStrategy);
            return AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(x => x.GetTypes())
              .Where(x => interfaceType.IsAssignableFrom(x)
                        && !x.IsInterface
                        && !x.IsAbstract)
              .Select(x => (IRuleOrchestrationStrategy)Activator.CreateInstance(x))
              .Where(s => s.ApplyStrategy(ruleCalculationMode))
              .OrderBy(x => x.OrchestrationOrder);
        }
    }
    public interface IRuleOrchestrationStrategy
    {
        bool ApplyStrategy(IRuleCalculationMode ruleCalculationMode);
        short OrchestrationOrder { get; }
        List<DetentionForOffence> FinalizeDetention(List<DetentionForOffence> detentions);
    }
    public class ConcurrentRuleStrategy : IRuleOrchestrationStrategy
    {
        public short OrchestrationOrder { get { return 1; } }

        public bool ApplyStrategy(IRuleCalculationMode ruleCalculationMode)
        {
            return ruleCalculationMode != null
            && ruleCalculationMode.CalculationType == RuleCalculationModeType.Concurrent;
        }
        public List<DetentionForOffence> FinalizeDetention(List<DetentionForOffence> detentions)
        {
            //I need more clarifications regarding this Mode of calculation
            //Not sure if 
            //1. I need to group all offences, calculate the detention per offence type and take maximum of these
            //2. I need to just take max Detention among all offence and return that.
            //3. Or I have not understood this at all.
            //Either ways, this method  needs to be updated to handle this logic.
            //Currently below I am using approach 2
            if (detentions != null && detentions.Count > 0)
            {
                return (detentions.Where(d => d != null).OrderByDescending(d => d.DetentionInHours).Take(1).ToList());
            }
            return detentions;
        }
    }
    public class ConsecutiveRuleStrategy : IRuleOrchestrationStrategy
    {
        public short OrchestrationOrder { get { return 0; } }
        public bool ApplyStrategy(IRuleCalculationMode ruleCalculationMode)
        {
            return ruleCalculationMode != null
            && ruleCalculationMode.CalculationType == RuleCalculationModeType.Consecutive;
        }
        public List<DetentionForOffence> FinalizeDetention(List<DetentionForOffence> detentions)
        {
            if (detentions != null && detentions.Count > 0)
            {
                return (detentions.Where(d => d != null).GroupBy(detention => detention.Offence).Select(group => new DetentionForOffence { Offence = group.Key, DetentionInHours = group.Sum(detention => detention.DetentionInHours) }).ToList<DetentionForOffence>());
            }
            return detentions;
        }
    }
    public interface IDetentionRule
    {
        IEnumerable<DetentionForOffence> GetDetention(IEnumerable<IOffence> offences, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> standardDetentions);
    }
    public abstract class BaseDetentionRule : IDetentionRule
    {
        public virtual IEnumerable<DetentionForOffence> GetDetention(IEnumerable<IOffence> offences, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> standardDetentions)
        {
            System.Collections.Generic.List<DetentionForOffence> detentionList = new System.Collections.Generic.List<DetentionForOffence>();
            if (offences != null && offences.Count() > 0)
            {
                offences.ToList().ForEach(offence => detentionList.Add(new DetentionForOffence { Offence = offence, DetentionInHours = standardDetentions.InternalList.Where(sd => sd.Offence.Id == offence.Id).FirstOrDefault().DetentionInHours }));
            }
            return detentionList;
        }
    }
    public abstract class PercentageDetentionRule : BaseDetentionRule, IDetentionRule
    {
        protected virtual double PercentageValue { get; }
        public override IEnumerable<DetentionForOffence> GetDetention(IEnumerable<IOffence> offences, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> standardDetentions)
        {
            return ModifyDetention(offences, standardDetentions);
        }
        public virtual IEnumerable<DetentionForOffence> ModifyDetention(IEnumerable<IOffence> offences, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> standardDetentions)
        {
            System.Collections.Generic.List<DetentionForOffence> detentionList = new System.Collections.Generic.List<DetentionForOffence>(base.GetDetention(offences, standardDetentions));
            if (detentionList != null && detentionList.Count > 0)
            {
                detentionList.ForEach(detention => detention.DetentionInHours = detention.DetentionInHours * (1 + (this.PercentageValue / 100.0)));
            }
            return detentionList;
        }
    }
    public class GoodStudentDetentionRule : PercentageDetentionRule, IDetentionRule
    {
        protected override double PercentageValue { get { return Constants.GoodStudentDetentionRebatePercentage; } }
        public override IEnumerable<DetentionForOffence> ModifyDetention(IEnumerable<IOffence> offences, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> standardDetentions)
        {
            if (offences != null && offences.Count() == 1)
            {
                return base.ModifyDetention(offences, standardDetentions);
            }
            else return null;
        }
    }
    public class BadStudentDetentionRule : PercentageDetentionRule, IDetentionRule
    {
        protected override double PercentageValue { get { return Constants.BadStudentDetentionSurplusPercentage; } }
        public override IEnumerable<DetentionForOffence> ModifyDetention(IEnumerable<IOffence> offences, IDEntityList<StandardDetentionForOffence, IStandardDetentionForOffence> standardDetentions)
        {
            if (offences != null && offences.Count() > 1)
            {
                return base.ModifyDetention(offences, standardDetentions);
            }
            else return null;
        }
    }
    //// Ideally Move these to a configuration table
    internal static class Constants
    {
        public const double BadStudentDetentionSurplusPercentage = 10.0;
        public const double GoodStudentDetentionRebatePercentage = -10.0;

        public const double DetentionPeriodLimitForADay = 8.0; 
    }

}
