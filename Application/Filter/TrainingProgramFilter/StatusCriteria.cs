using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Filter.TrainingProgramFilter
{
    public class StatusCriteria : ICriterias<TrainingProgram>
    {
        private string? searchCriteria;
        public StatusCriteria(string? searchCriteria)
        {
            this.searchCriteria = searchCriteria;
        }
        public List<TrainingProgram> MeetCriteria(List<TrainingProgram> trainingPrograms)
        {
            List<TrainingProgram> trainingProgramData = new List<TrainingProgram>();
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (TrainingProgram tp in trainingPrograms)
                    if (tp.Status.ToLower().Equals(searchCriteria.ToLower()))
                        trainingProgramData.Add(tp);
                return trainingProgramData;
            }
            else
                return trainingPrograms;
        }
    }
}
