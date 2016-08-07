using System;
using System.Collections.Generic;
using System.Linq;
using Nalasha.DetentionCalculator.Core.Entities;

namespace Nalasha.DetentionCalculator.Core.Processors
{
    public interface IDetentionCalculator
    {
        ICalculateDetentionResponse Calculate(ICalculateDetentionRequest request);
    }
    public class DetentionCalculator : IDetentionCalculator
    {
        public DetentionCalculator()
        {

        }
        public ICalculateDetentionResponse Calculate(ICalculateDetentionRequest request)
        {
            throw new NotImplementedException();
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
                        && !x.IsAbstract
                        && ((IRuleOrchestrationStrategy)x).ApplyStrategy(ruleCalculationMode))
              .Select(x => (IRuleOrchestrationStrategy)Activator.CreateInstance(x));
        }
    }
    public interface IRuleOrchestrationStrategy
    {
        bool ApplyStrategy(IRuleCalculationMode ruleCalculationMode);
        List<DetentionForOffence> FinalizeDetention(List<DetentionForOffence> detentions);
    }
    public class ConcurrentRuleStrategy : IRuleOrchestrationStrategy
    {
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
                return (detentions.OrderByDescending(d => d.DetentionInHours).Take(1).ToList());
            }
            return detentions;
        }
    }
    public class ConsecutiveRuleStrategy : IRuleOrchestrationStrategy
    {
        public bool ApplyStrategy(IRuleCalculationMode ruleCalculationMode)
        {
            return ruleCalculationMode != null
            && ruleCalculationMode.CalculationType == RuleCalculationModeType.Consecutive;
        }
        public List<DetentionForOffence> FinalizeDetention(List<DetentionForOffence> detentions)
        {
            if (detentions != null && detentions.Count > 0)
            {
                return (detentions.GroupBy(detention => detention.Offence).Select(group => new DetentionForOffence { Offence = group.Key, DetentionInHours = group.Sum(detention => detention.DetentionInHours) }).ToList<DetentionForOffence>());
            }
            return detentions;
        }
    }
    public interface IDetentionRule
    {
        IEnumerable<DetentionForOffence> GetDetention(List<IOffence> offences, System.Collections.Generic.IEnumerable<IStandardDetentionForOffence> standardDetentions);
    }
    public abstract class BaseDetentionRule : IDetentionRule
    {
        public virtual IEnumerable<DetentionForOffence> GetDetention(List<IOffence> offences, System.Collections.Generic.IEnumerable<IStandardDetentionForOffence> standardDetentions)
        {
            System.Collections.Generic.List<DetentionForOffence> detentionList = new System.Collections.Generic.List<DetentionForOffence>();
            if (offences != null && offences.Count > 0)
            {
                offences.ForEach(offence => detentionList.Add(new DetentionForOffence { Offence = offence, DetentionInHours = standardDetentions.Where(sd => sd.Offence.Id == offence.Id).FirstOrDefault().DetentionInHours }));
            }
            return detentionList;
        }
    }
    public abstract class PercentageDetentionRule : BaseDetentionRule, IDetentionRule
    {
        protected virtual int PercentageValue { get; }
        public virtual IEnumerable<DetentionForOffence> ModifyDetention(List<IOffence> offences, System.Collections.Generic.IEnumerable<IStandardDetentionForOffence> standardDetentions)
        {
            System.Collections.Generic.List<DetentionForOffence> detentionList = new System.Collections.Generic.List<DetentionForOffence>(base.GetDetention(offences, standardDetentions));
            if (detentionList != null && detentionList.Count > 0)
            {
                detentionList.ForEach(detention => detention.DetentionInHours = detention.DetentionInHours * (1 + (this.PercentageValue / 100)));
            }
            return detentionList;
        }
    }
    public class GoodStudentDetentionRule : PercentageDetentionRule, IDetentionRule
    {
        protected override int PercentageValue { get { return Constants.GoodStudentDetentionRebatePercentage; } }
        public override IEnumerable<DetentionForOffence> ModifyDetention(List<IOffence> offences, IEnumerable<IStandardDetentionForOffence> standardDetentions)
        {
            if (offences != null && offences.Count == 1)
            {
                return base.ModifyDetention(offences, standardDetentions);
            }
            else return null;
        }
    }
    public class BadStudentDetentionRule : PercentageDetentionRule, IDetentionRule
    {
        protected override int PercentageValue { get { return Nalasha.DetentionCalculator.Core.Processors.Constants.BadStudentDetentionSurplusPercentage; } }
        public override IEnumerable<DetentionForOffence> ModifyDetention(List<IOffence> offences, IEnumerable<IStandardDetentionForOffence> standardDetentions)
        {
            if (offences != null && offences.Count > 1)
            {
                return base.ModifyDetention(offences, standardDetentions);
            }
            else return null;
        }
    }
    internal static class Constants
    {
        public const int BadStudentDetentionSurplusPercentage = 10;
        public const int GoodStudentDetentionRebatePercentage = -10;
    }

}
