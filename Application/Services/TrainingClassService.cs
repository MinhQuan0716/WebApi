using Application.Filter.ClassFilter;
using Application.Interfaces;
using Application.ViewModels.TrainingClassModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
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
        /// This method set the isDeleted value of object to true
        /// </summary>
        /// <param name="trainingClassId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> SoftRemoveTrainingClass(string trainingClassId)
        {
            var trainingClassObj = await GetTrainingClassByIdAsync(trainingClassId);

            _unitOfWork.TrainingClassRepository.SoftRemove(trainingClassObj);
            return (await _unitOfWork.SaveChangeAsync() > 0);
        }
        public async Task<List<TrainingClass>> SearchClassByName(string name)
        {
            var listClass = _unitOfWork.TrainingClassRepository.SearchClassByName(name);
            return listClass;
        }
        public async Task<bool> DuplicateClass(Guid id)
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
                    Code=result.Code,
                    Attendee=result.Attendee,   
                    Branch=result.Branch,
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
        public async Task<TrainingClassViewModel?> CreateTrainingClassAsync(CreateTrainingClassDTO createTrainingClassDTO)
        {
            try
            {
                var trainingClassObj = _mapper.Map<TrainingClass>(createTrainingClassDTO);
                await _unitOfWork.TrainingClassRepository.AddAsync(trainingClassObj);
                
                //set location
                trainingClassObj.Location = await _unitOfWork.LocationRepository.GetByIdAsync(createTrainingClassDTO.LocationID) ?? throw new Exception("Invalid location Id");

                //set training program
                trainingClassObj.TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(createTrainingClassDTO.TrainingProgramId) ?? throw new Exception("Invalid training program Id");

                return (await _unitOfWork.SaveChangeAsync() > 0) ? _mapper.Map<TrainingClassViewModel>(trainingClassObj) : null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// This method uses _classId to find and update that class
        /// </summary>
        /// <param name="_classId"></param>
        /// <param name="updateTrainingCLassDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTrainingClass(string trainingClassId, UpdateTrainingCLassDTO updateTrainingCLassDTO)
        {
            var trainingClassObj= await GetTrainingClassByIdAsync(trainingClassId);
            _mapper.Map<UpdateTrainingCLassDTO, TrainingClass>(updateTrainingCLassDTO, trainingClassObj);
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
        /// <param name="trainingClassId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                throw new AutoMapperMappingException("Id is not a guid");
            }
        }

        public async Task<List<TrainingClassDTO>> GetAllTrainingClassesAsync()
        {
            var trainingClasses = _unitOfWork.TrainingClassRepository.GetTrainingClasses();
            return trainingClasses;
        }


        public async Task<List<TrainingClassDTO>> FilterLocation(string[]? locationName, string branchName, DateTime? date1, DateTime? date2, string[]? classStatus, string[]?attendInClass)
        {
            ICriterias<TrainingClassDTO> locationCriteria = new LocationCriteria(locationName);
            ICriterias<TrainingClassDTO> dateCriteria = new DateCriteria(date1, date2);
            ICriterias<TrainingClassDTO> branchCriteria = new ClassBranchCriteria(branchName);
            ICriterias<TrainingClassDTO> statusCriteria=new StatusClassCriteria(classStatus);
            ICriterias<TrainingClassDTO> attendCriteria=new AttendeeCriteria(attendInClass);    
            ICriterias<TrainingClassDTO> andCirteria = new AndClassFilter(dateCriteria, locationCriteria,branchCriteria, statusCriteria,attendCriteria);
            var getAll = _unitOfWork.TrainingClassRepository.GetTrainingClasses();
            var filterResult = andCirteria.MeetCriteria(getAll);
            return filterResult;
        }

    
    }
}
