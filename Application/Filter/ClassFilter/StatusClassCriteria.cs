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
    public  class StatusClassCriteria:ICriterias<TrainingClassDTO>
    {
        public string[]? statusClass;
        public StatusClassCriteria(string[]? statusClass){ 
        this.statusClass = statusClass;
        }
        public List<TrainingClassDTO> MeetCriteria(List<TrainingClassDTO> classList)
        {
            if (!statusClass.IsNullOrEmpty())
            {
                List<TrainingClassDTO> classData = new List<TrainingClassDTO>();
                for (int i = 0; i <= statusClass.Length; i++)
                {
                    foreach (TrainingClassDTO item in classList)
                    {
                        if (statusClass[i].ToLower().Equals(item.Status.ToLower()))
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
