using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Filter.TrainingProgramFilter
{
    public class CreatedByCriteria : ICriterias<TrainingProgram>
    {
        private List<User>? listUsers;
        private string? searchCriteria;
        public CreatedByCriteria(List<User>? listUsers,string? searchCriteria)
        {
            this.listUsers = listUsers;
            this.searchCriteria = searchCriteria;
        }
        public List<TrainingProgram> MeetCriteria(List<TrainingProgram> trainingPrograms)
        {
            List<TrainingProgram> trainingProgramData = new List<TrainingProgram>();
            if (searchCriteria?.Any() == true)
            {
                foreach (TrainingProgram tp in trainingPrograms)
                    foreach (User user in listUsers)
                    {
                        if (!user.FullName.IsNullOrEmpty())
                        {
                            if (user.FullName.ToLower().Contains(searchCriteria.ToLower()))
                            {
                                if (tp.CreatedBy.Equals(user.Id))
                                    trainingProgramData.Add(tp);
                            }
                        }
                        if (user.UserName.ToLower().Contains(searchCriteria.ToLower()))
                        {
                            if (tp.CreatedBy.Equals(user.Id))
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
