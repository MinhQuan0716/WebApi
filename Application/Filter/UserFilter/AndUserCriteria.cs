using Application.Interfaces;
using Domain.Entities;

namespace Application.Filter.UserFilter
{
    public class AndUserCriteria : ICriterias<User>
    {
        private ICriterias<User> criteria;
        private ICriterias<User> otherCriteria;

        public AndUserCriteria(ICriterias<User> criteria, ICriterias<User> otherCriteria)
        {
            this.criteria = criteria;
            this.otherCriteria = otherCriteria;
        }

        public List<User> MeetCriteria(List<User> users)
        {
            List<User> firstCriteriaUsers = criteria.MeetCriteria(users);
            return otherCriteria.MeetCriteria(firstCriteriaUsers);
        }
    }
}
