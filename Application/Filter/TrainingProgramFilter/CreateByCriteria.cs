using Application.Interfaces;
using Domain.Entities;

namespace Application.Filter.TrainingProgramFilter
{
    public class CreateByCriteria : ICriterias<TrainingProgram>
    {
        private Guid? searchCriteria;
        public CreateByCriteria(Guid? searchCriteria)
        {
            this.searchCriteria = searchCriteria;
        }
        public List<TrainingProgram> MeetCriteria(List<TrainingProgram> trainingPrograms)
        {
            if (searchCriteria != Guid.Empty)
            {
                List<TrainingProgram> trainingProgramData = new List<TrainingProgram>();
                foreach (TrainingProgram tp in trainingPrograms)
                {
                    if (tp.CreatedBy.Equals(searchCriteria))
                    {
                        trainingProgramData.Add(tp);
                    }
                }
                return trainingProgramData;
            }
            else
                return trainingPrograms;
        }
    }
}
