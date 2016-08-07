using DetentionCalculator.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetentionCalculator.Core
{
    public interface IBaseConverter<S, D>
    {
        D Convert(S source);
    }
    public class DetentionForOffenceToStudentDetentionConverter : IBaseConverter<ICalculateDetentionResponse, IEnumerable<IStudentDetention>>
    {
        public IEnumerable<IStudentDetention> Convert(ICalculateDetentionResponse source)
        {
            if(source != null && source.CalculateDetentionRequest != null && source.DetentionList != null && source.DetentionList.Count > 0)
            {
                DateTime detentionStartTime = source.CalculateDetentionRequest.DetentionStartTime;
                List<StudentDetention> result = new List<StudentDetention>();
                source.DetentionList.ForEach(d =>
                {
                    result.Add(new StudentDetention
                    {
                        DetentionInHours = d.DetentionInHours,
                        DetentionStartTime = detentionStartTime,
                        DetentionEndTime = detentionStartTime.AddHours(d.DetentionInHours)
                    });
                    detentionStartTime = detentionStartTime.AddHours(d.DetentionInHours);
                }
                );
                return result;
            }
        return null;
        }
    }
}
