using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public ISyllabusRepository SyllabusRepository { get; }
        public IUserRepository UserRepository { get; }
        public IUnitRepository UnitRepository { get; }
        public ILectureRepository LectureRepository { get; }
        public IDetailUnitLectureRepository DetailUnitLectureRepository { get; }

        public ITrainingMaterialRepository TrainingMaterialRepository { get; }
        public IApplicationReapository ApplicationReapository { get; }
        public IAttendanceRepository AttendanceRepository { get; }
        public ITrainingClassRepository TrainingClassRepository { get; }
        public ILocationRepository LocationRepository { get; }
        public IFeedbackRepository FeedbackRepository { get; }
        public IDetailTrainingProgramSyllabusRepository DetailTrainingProgramSyllabusRepository { get; }
        public ITrainingProgramRepository TrainingProgramRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
