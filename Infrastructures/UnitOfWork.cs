using Application;
using Application.Repositories;
using Infrastructures.Repositories;
using System.Runtime.CompilerServices;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISyllabusRepository _syllabusRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly ILectureRepository _lectureRepository;
        private readonly IDetailUnitLectureRepository _detailUnitLectureRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ITrainingMaterialRepository _trainingMaterialRepository;


        private readonly ITrainingClassRepository _trainingClassRepository;
        public readonly ILocationRepository _locationRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly IDetailTrainingProgramSyllabusRepository _detailTrainingProgramSyllabusRepository;
        private readonly IGradingRepository _gradingRepository;

        public UnitOfWork(AppDbContext dbContext,
            IUserRepository userRepository, ITrainingMaterialRepository trainingMaterialRepository,
            ISyllabusRepository syllabusRepository, IUnitRepository unitRepository, ILectureRepository lectureRepository, IDetailUnitLectureRepository detailUnitLectureRepository, ITrainingClassRepository trainingClassRepository, ILocationRepository locationRepository,
            IFeedbackRepository feedbackRepository, ITrainingProgramRepository trainingProgramRepository, IDetailTrainingProgramSyllabusRepository detailTrainingProgramSyllabusRepository, IAttendanceRepository attendanceRepository, IApplicationRepository applicationReapository,
            IGradingRepository gradingRepository)

        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _syllabusRepository = syllabusRepository;
            _unitRepository = unitRepository;
            _lectureRepository = lectureRepository;
            _detailUnitLectureRepository = detailUnitLectureRepository;

            _trainingMaterialRepository = trainingMaterialRepository;

            _trainingClassRepository = trainingClassRepository;
            _locationRepository = locationRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _detailTrainingProgramSyllabusRepository = detailTrainingProgramSyllabusRepository;
            _feedbackRepository = feedbackRepository;
            _attendanceRepository = attendanceRepository;
            _applicationRepository = applicationReapository;
            _trainingProgramRepository = trainingProgramRepository;
            _detailTrainingProgramSyllabusRepository = detailTrainingProgramSyllabusRepository;
            _gradingRepository = gradingRepository;
        }
        public IUserRepository UserRepository => _userRepository;
        public ISyllabusRepository SyllabusRepository => _syllabusRepository;



        public IUnitRepository UnitRepository => _unitRepository;
        public ILectureRepository LectureRepository => _lectureRepository;



        public IDetailUnitLectureRepository DetailUnitLectureRepository => _detailUnitLectureRepository;


        public ITrainingMaterialRepository TrainingMaterialRepository => _trainingMaterialRepository;


        public ITrainingClassRepository TrainingClassRepository => _trainingClassRepository;
        public ILocationRepository LocationRepository => _locationRepository;

        public ITrainingProgramRepository TrainingProgramRepository => _trainingProgramRepository;
        public IDetailTrainingProgramSyllabusRepository DetailTrainingProgramSyllabusRepository => _detailTrainingProgramSyllabusRepository;
        public IFeedbackRepository FeedbackRepository => _feedbackRepository;

        public IAttendanceRepository AttendanceRepository => _attendanceRepository;

        public IApplicationRepository ApplicationRepository => _applicationRepository;

        public IGradingRepository GradingRepository => _gradingRepository;



        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

