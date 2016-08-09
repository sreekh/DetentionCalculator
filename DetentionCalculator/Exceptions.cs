using DetentionCalculator.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetentionCalculator.Core
{
    public interface IDetentionExceedsDayLimitException
    {
        ICalculateDetentionResponse CalculateDetentionResponse { get; set; }
    }
    public class DetentionExceedsDayLimitException : Exception
    {
        public ICalculateDetentionResponse CalculateDetentionResponse { get; set; }
        public DetentionExceedsDayLimitException(ICalculateDetentionResponse calculateDetentionResponse)
            :base("Detention calculated exceeds specified working hours limit for a day.")
        {
            this.CalculateDetentionResponse = calculateDetentionResponse;
        }
    }
}
