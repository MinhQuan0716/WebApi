using Application.Interfaces;
using Domain.Entities;

namespace Application.Filter.UserFilter
{
    public class RoleCriteria : ICriterias<User>
    {
        private int? searchCriteria;
        public RoleCriteria(int? searchCriteria)
        {
            this.searchCriteria = searchCriteria;
        }
        public List<User> MeetCriteria(List<User> users)
        {
            List<User> userData = new List<User>();
            if (searchCriteria != null)
            {
                foreach (User user in users)
                    if (user.RoleId.Equals(searchCriteria))
                        userData.Add(user);
                return userData;
            }
            else
                return users;
        }
    }
}
