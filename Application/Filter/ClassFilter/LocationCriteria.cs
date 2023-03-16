using Application.Interfaces;
using Application.ViewModels.TrainingClassModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Filter.ClassFilter
{
    public class LocationCriteria : ICriterias<TrainingClassDTO>
    {
        public string[]? locationName;
        public LocationCriteria(string[]? locationName)
        {
            this.locationName = locationName;
        }

        public List<TrainingClassDTO> MeetCriteria(List<TrainingClassDTO> classList)
        {
            if (!locationName.IsNullOrEmpty())
            {
                List<TrainingClassDTO> classData = new List<TrainingClassDTO>();
                for (int i = 0; i <= locationName.Length; i++)
                {
                    foreach (TrainingClassDTO item in classList)
                    {
                        if (locationName[i].ToLower().Equals(item.LocationName.ToLower()))
                        {
                            classData.Add(item);
                        }

                    }
                    return classData;
                }
            }
            return classList;
        }
    }
}
