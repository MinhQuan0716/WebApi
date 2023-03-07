using Application.Interfaces;
using Application.ViewModels.TrainingClassModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Filter.ClassFilter
{
    public class AndClassFilter : ICriterias<TrainingClassDTO>
    {
        private ICriterias<TrainingClassDTO> firstCriterias;
        private ICriterias<TrainingClassDTO> secondCriterias;
       public AndClassFilter(ICriterias<TrainingClassDTO> firstCriteria,ICriterias<TrainingClassDTO> secondCriteria)
        {
            this.firstCriterias = firstCriteria;
            this.secondCriterias = secondCriteria;
        }

        public List<TrainingClassDTO> MeetCriteria(List<TrainingClassDTO> classlist)
        {
            List<TrainingClassDTO> trainingClassDTOs= firstCriterias.MeetCriteria(classlist);
            return secondCriterias.MeetCriteria(trainingClassDTOs);
        }
    }
}
