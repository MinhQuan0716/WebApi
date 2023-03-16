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
    public class ClassBranchCriteria : ICriterias<TrainingClassDTO>
    {
        string? branchName;
        public ClassBranchCriteria(string? branchName)
        {
            this.branchName = branchName;
        }

        public List<TrainingClassDTO> MeetCriteria(List<TrainingClassDTO> classList)
        {
            if (!branchName.IsNullOrEmpty())
            {
                List<TrainingClassDTO> trainingClassDTOs = new List<TrainingClassDTO>();
                foreach (TrainingClassDTO classDTO in classList)
                {
                    if (classDTO.Branch.ToLower().Equals(branchName.ToLower()))
                    {
                        trainingClassDTOs.Add(classDTO);
                    }
                }
                return trainingClassDTOs;
            }
            return classList;
        }
    }
}
