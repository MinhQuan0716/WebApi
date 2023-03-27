namespace Application.ViewModels.UserViewModels
{
    public class UserSearchFilterModels
    {
        public class UserFilterModel
        {
            public string Gender { get; set; }
            public int Role { get; set; }
            public string Level { get; set; }
        }
        public class UserSearchModel
        {
            public string Keyword { get; set; }
        }
    }
}
