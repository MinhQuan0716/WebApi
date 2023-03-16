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
        private ICriterias<TrainingClassDTO> thirdCriteria;
        private ICriterias<TrainingClassDTO> fourthCriteria;
        private ICriterias<TrainingClassDTO> fifthCriteria;
        public AndClassFilter(ICriterias<TrainingClassDTO> firstCriteria, ICriterias<TrainingClassDTO> secondCriteria, ICriterias<TrainingClassDTO> thirdCriteria, ICriterias<TrainingClassDTO> fourthCriteria, ICriterias<TrainingClassDTO> fifthCriteria)
        {
            this.firstCriterias = firstCriteria;
            this.secondCriterias = secondCriteria;
            this.thirdCriteria = thirdCriteria;
            this.fourthCriteria = fourthCriteria;
            this.fifthCriteria = fifthCriteria;
        }

        public List<TrainingClassDTO> MeetCriteria(List<TrainingClassDTO> classlist)
        {
            List<TrainingClassDTO> firstResultList = firstCriterias.MeetCriteria(classlist);
            List<TrainingClassDTO> secondResultList = secondCriterias.MeetCriteria(firstResultList);
            List<TrainingClassDTO> thirdResultList = thirdCriteria.MeetCriteria(secondResultList);
            List<TrainingClassDTO> fourthResultList = fourthCriteria.MeetCriteria(thirdResultList);
            return fifthCriteria.MeetCriteria(fourthResultList);
        }
    }
}
