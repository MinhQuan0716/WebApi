using Application.Filter.ClassFilter;
using Application.Interfaces;
using Application.ViewModels.SyllabusModels;
using Application.ViewModels.TrainingClassModels;
using Application.ViewModels.TrainingProgramModels;
using Application.ViewModels.TrainingProgramModels.TrainingProgramView;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// Training Class Services
    /// </summary>
    public class TrainingClassService : ITrainingClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainingClassService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// SoftRemoveTrainingClassAsync set the isDeleted value of training class to true
        /// </summary>
        /// <param className="trainingClassId">training class ID</param>
        /// <returns>True if save succesful, false if save fail</returns>
        public async Task<bool> SoftRemoveTrainingClassAsync(string trainingClassId)
        {
            var trainingClassObj = await GetTrainingClassByIdAsync(trainingClassId);

            _unitOfWork.TrainingClassRepository.SoftRemove(trainingClassObj);
            return (await _unitOfWork.SaveChangeAsync() > 0);
        }

        /// <summary>
        /// SearchClassByNameAsync return classes by class className
        /// </summary>
        /// <param className="className">Training class name</param>
        /// <returns>List of training classes<TrainingClass></returns>
        public async Task<List<TrainingClass>> SearchClassByNameAsync(string className)
        {
            var listClass = _unitOfWork.TrainingClassRepository.SearchClassByName(className);
            return listClass;
        }

        /// <summary>
        /// DuplicateClassAsync duplicate an existed training class
        /// </summary>
        /// <param name="id">Training class ID</param>
        /// <returns>True if training class existed, false otherwise</returns>
        public async Task<bool> DuplicateClassAsync(Guid id)
        {
            var result = await _unitOfWork.TrainingClassRepository.GetByIdAsync(id);
            if (result != null)
            {
                TrainingClass trainingClass = new TrainingClass()
                {
                    Name = result.Name,
                    StartTime = result.StartTime,
                    EndTime = result.EndTime,
                    CreatedBy = result.CreatedBy,
                    Code = result.Code,
                    Attendee = result.Attendee,
                    Branch = result.Branch,
                    CreationDate = result.CreationDate,
                    LocationID = result.LocationID,
                    DeleteBy = result.DeleteBy,
                    DeletionDate = result.DeletionDate,
                    IsDeleted = result.IsDeleted,
                    TrainingProgramId = result.TrainingProgramId,
                    StatusClassDetail = result.StatusClassDetail,

                };
                await _unitOfWork.TrainingClassRepository.AddAsync(trainingClass);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// CreateTrainingClassAsync add new training class to the database
        /// </summary>
        /// <param name="createTrainingClassDTO">Create training class DTO</param>
        /// <returns>Training class view model</returns>
        public async Task<TrainingClassViewModel?> CreateTrainingClassAsync(CreateTrainingClassDTO createTrainingClassDTO)
        {
            var trainingClassObj = _mapper.Map<TrainingClass>(createTrainingClassDTO);
            await _unitOfWork.TrainingClassRepository.AddAsync(trainingClassObj);

            //set location
            trainingClassObj.Location = await _unitOfWork.LocationRepository.GetByIdAsync(createTrainingClassDTO.LocationID) ?? throw new Exception("Invalid location Id");

            //set training program
            trainingClassObj.TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(createTrainingClassDTO.TrainingProgramId) ?? throw new Exception("Invalid training program Id");

            return (await _unitOfWork.SaveChangeAsync() > 0) ? _mapper.Map<TrainingClassViewModel>(trainingClassObj) : null;
        }

        /// <summary>
        /// UpdateTrainingClassAsync update training class based on its id
        /// </summary>
        /// <param className="trainingClassId">Training class ID</param>
        /// <param className="updateTrainingCLassDTO">Update training class DTO</param>
        /// <returns>True if save successfully, false if save fail</returns>
        public async Task<bool> UpdateTrainingClassAsync(string trainingClassId, UpdateTrainingCLassDTO updateTrainingCLassDTO)
        {
            var trainingClassObj = await GetTrainingClassByIdAsync(trainingClassId);
            _mapper.Map(updateTrainingCLassDTO, trainingClassObj);
            //set location
            trainingClassObj.Location = await _unitOfWork.LocationRepository.GetByIdAsync(updateTrainingCLassDTO.LocationID) ?? throw new NullReferenceException("Invalid location Id");

            //set training program
            trainingClassObj.TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(updateTrainingCLassDTO.TrainingProgramId) ?? throw new NullReferenceException("Invalid training program Id");

            _unitOfWork.TrainingClassRepository.Update(trainingClassObj);
            return (await _unitOfWork.SaveChangeAsync() > 0);
        }

        /// <summary>
        /// This method find, return Training class and throw exception if can't find or get a mapping exception
        /// </summary>
        /// <param className="trainingClassId">Training class ID</param>
        /// <returns>Training class</returns>
        /// <exception cref="AutoMapperMappingException">When training class ID is not a guid</exception>
        public async Task<TrainingClass> GetTrainingClassByIdAsync(string trainingClassId)
        {
            try
            {
                var _classId = _mapper.Map<Guid>(trainingClassId);
                var trainingClassObj = await _unitOfWork.TrainingClassRepository.GetByIdAsync(_classId);
                if (trainingClassObj == null)
                {
                    throw new NullReferenceException("Incorrect Id");
                }
                return trainingClassObj;
            }
            catch (AutoMapperMappingException)
            {
                throw new AutoMapperMappingException("Id must be a guid");
            }
        }

        /// <summary>
        /// GetAllTrainingClassesAsync returns all training classes
        /// </summary>
        /// <returns>List of training class</returns>
        public async Task<List<TrainingClassDTO>> GetAllTrainingClassesAsync()
        {
            var trainingClasses = _unitOfWork.TrainingClassRepository.GetTrainingClasses();
            return trainingClasses;
        }


        public async Task<List<TrainingClassDTO>> FilterLocation(string[]? locationName, string branchName, DateTime? date1, DateTime? date2, string[]? classStatus, string[]? attendInClass)
        {
            ICriterias<TrainingClassDTO> locationCriteria = new LocationCriteria(locationName);
            ICriterias<TrainingClassDTO> dateCriteria = new DateCriteria(date1, date2);
            ICriterias<TrainingClassDTO> branchCriteria = new ClassBranchCriteria(branchName);
            ICriterias<TrainingClassDTO> statusCriteria = new StatusClassCriteria(classStatus);
            ICriterias<TrainingClassDTO> attendCriteria = new AttendeeCriteria(attendInClass);
            ICriterias<TrainingClassDTO> andCirteria = new AndClassFilter(dateCriteria, locationCriteria, branchCriteria, statusCriteria, attendCriteria);
            var getAll = _unitOfWork.TrainingClassRepository.GetTrainingClasses();
            var filterResult = andCirteria.MeetCriteria(getAll);
            return filterResult;
        }

        public async  Task<FinalTrainingClassDTO> GetFinalTrainingClassesAsync(Guid id)
        {
          FinalTrainingClassDTO finalDTO =new FinalTrainingClassDTO();
            var trainingClassDetail = await _unitOfWork.TrainingClassRepository.GetByIdAsync(id);
           var trainingClassViewAllDTO = _unitOfWork.TrainingClassRepository.GetTrainingClasses();
            var trainingProgram = _unitOfWork.TrainingClassRepository.GetTrainingProgramByClassID(id);
            var trainingClassDTO = _mapper.Map<TrainingClassViewDetail>(trainingClassDetail);
            var detailProgramSyllabus=_unitOfWork.DetailTrainingProgramSyllabusRepository.GetDetailByClassID(trainingProgram.Id);
            foreach (TrainingClassDTO trainingClasses in trainingClassViewAllDTO)
            {

                AttendeeDTO attendeeDTO = new AttendeeDTO()
                {
                    Attendee = trainingClasses.Attendee
                };
                CreatedByDTO createdByDTO = new CreatedByDTO()
                {
                    creationDate = trainingClasses.CreationDate,
                    userName = trainingClasses.CreatedBy
                };
                TrainingProgramViewForTrainingClassDetail trainingProgramViewModel = new TrainingProgramViewForTrainingClassDetail()
                {
                    programId = trainingProgram.Id,
                    programName = trainingProgram.ProgramName,
                    programDuration = new DurationView
                    {
                        TotalHours = trainingProgram.Duration
                    }

                };
                var syllabusDetail = await _unitOfWork.SyllabusRepository.FindAsync(x => x.Id == detailProgramSyllabus.SyllabusId);
                foreach (Syllabus syllabus in syllabusDetail)
                {
                    var syllabusDTO = _mapper.Map<SyllabusViewForTrainingClassDetail>(syllabus);
                    List<SyllabusViewForTrainingClassDetail> syllabusViewAllDTOs = new List<SyllabusViewForTrainingClassDetail>();
                    syllabusViewAllDTOs.Add(syllabusDTO);
                    finalDTO.syllabusDTO = syllabusViewAllDTOs;
                }
                finalDTO.TrainingClass = trainingClassDTO;
                finalDTO.location = trainingClasses.LocationName;
                finalDTO.FSU = trainingClasses.Branch;
                finalDTO.general = new GeneralTrainingClassDTO
                {
                    class_date = new ClassDateDTO
                    {
                        StartDate = trainingClasses.StartDate,
                        EndDate = trainingClasses.EndDate,
                    }
                };
                finalDTO.attendeeDTO = attendeeDTO;
                finalDTO.createdDTO = createdByDTO;
                finalDTO.programModel = trainingProgramViewModel;
            }
            
            return finalDTO;
        }

    }
}
