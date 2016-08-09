using DetentionCalculator.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetentionCalculator.Core
{
    public interface IDetentionForOffenceToCalculateDetentionResponseConverter
    {
        ICalculateDetentionResponse Convert(IEnumerable<DetentionForOffence> detentionForOffenceList, ICalculateDetentionRequest request);
    }
    public  class DetentionForOffenceToCalculateDetentionResponseConverter : IDetentionForOffenceToCalculateDetentionResponseConverter
    {
        public ICalculateDetentionResponse Convert(IEnumerable<DetentionForOffence> detentionForOffenceList, ICalculateDetentionRequest request)
        {
            if (request != null && detentionForOffenceList != null && detentionForOffenceList.Count() > 0)
            {
                CalculateDetentionResponse response = new CalculateDetentionResponse { CalculateDetentionRequest = request, StudentDetentionList = new StudentDetentionList() };


                DateTime detentionStartTime = request.DetentionStartTime;
                detentionForOffenceList.ToList().ForEach(d =>
                {
                    response.StudentDetentionList.InternalList.Add(new StudentDetention
                    {
                        DetentionInHours = d.DetentionInHours,
                        DetentionStartTime = detentionStartTime,
                        DetentionEndTime = detentionStartTime.AddHours(d.DetentionInHours)
                    });
                    detentionStartTime = detentionStartTime.AddHours(d.DetentionInHours);
                }
                );
                return response;
            }
            return null;
        }
    }
}
