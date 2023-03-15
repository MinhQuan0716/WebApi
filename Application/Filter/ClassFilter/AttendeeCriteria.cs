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
    public class AttendeeCriteria : ICriterias<TrainingClassDTO>
    {
        public string[]? attendInClass;
        public AttendeeCriteria(string[]? attendInClass)
        {
            this.attendInClass = attendInClass;
        }

        public List<TrainingClassDTO> MeetCriteria(List<TrainingClassDTO> classList)
        {

            if (!attendInClass.IsNullOrEmpty())
            {
                List<TrainingClassDTO> classData = new List<TrainingClassDTO>();
                for (int i = 0; i <= attendInClass.Length; i++)
                {
                    foreach (TrainingClassDTO item in classList)
                    {
                        if (attendInClass[i].ToLower().Equals(item.Status.ToLower()))
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
