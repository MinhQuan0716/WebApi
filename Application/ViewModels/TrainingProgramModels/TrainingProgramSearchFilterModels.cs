namespace Application.ViewModels.TrainingProgramModels
{
    public class TrainingProgramSearchFilterModels
    {
        public class SearchTrainingProgramModel
        {
            public string? Keyword { get; set; }
        }
        public class FilterTrainingProgramModel
        {
            public string? CreateBy { get; set; }
            public string? Status { get; set; }
        }
    }
}
