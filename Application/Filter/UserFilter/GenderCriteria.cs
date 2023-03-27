using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Filter.UserFilter
{
    public class GenderCriteria : ICriterias<User>
    {
        private string searchCriteria;
        public GenderCriteria(string searchCriteria)
        {
            this.searchCriteria = searchCriteria;
        }
        public List<User> MeetCriteria(List<User> users)
        {
            List<User> userData = new List<User>();
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (User user in users)
                    if (user.Gender.IsNullOrEmpty() || user.Gender.ToLower().Equals(searchCriteria.ToLower()))
                        userData.Add(user);
                return userData;
            }
            else
                return users;
        }
    }
}
