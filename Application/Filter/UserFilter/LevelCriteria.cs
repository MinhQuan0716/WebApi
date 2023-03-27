using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Filter.UserFilter
{
    public class LevelCriteria : ICriterias<User>
    {
        private string searchCriteria;
        public LevelCriteria(string searchCriteria)
        {
            this.searchCriteria = searchCriteria;
        }
        public List<User> MeetCriteria(List<User> users)
        {
            List<User> userData = new List<User>();
            if (!searchCriteria.IsNullOrEmpty())
            {
                foreach (User user in users)
                    if ((user.Level.IsNullOrEmpty() || user.Level.ToLower().Equals(searchCriteria.ToLower())))
                        userData.Add(user);
                return userData;
            }
            else
                return users;
        }
    }
}
