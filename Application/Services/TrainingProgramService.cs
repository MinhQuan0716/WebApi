using Application.Filter.TrainingProgramFilter;
using Application.Interfaces;
using Application.ViewModels.TrainingProgramModels;
using Application.ViewModels.TrainingProgramModels.TrainingProgramView;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainingProgramService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TrainingProgramService(IUnitOfWork unitOfWork, IMapper mapper) : this(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<TrainingProgramViewModel> GetTrainingProgramDetail(Guid id)
        {
            var result = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(id);
            if (result is not null && result.IsDeleted == false)
            {
                var trainingProgramView = _mapper.Map<TrainingProgramViewModel>(result);
                if (trainingProgramView.TrainingProgramId is not null)
                    trainingProgramView.Contents = _mapper.Map<ICollection<SyllabusTrainingProgramViewModel>>((ICollection<Syllabus>?)await _unitOfWork.SyllabusRepository.GetSyllabusByTrainingProgramId(trainingProgramView.TrainingProgramId.Value));

                return trainingProgramView;
            }
            return null;
        }

        public async Task<TrainingProgram> CreateTrainingProgram(CreateTrainingProgramDTO createTrainingProgramDTO)
        {
            var syllabusesId = createTrainingProgramDTO.SyllabusesId;
            if (syllabusesId is not null)
            {


                // Create Training Program
                var trainingProgram = _mapper.Map<TrainingProgram>(createTrainingProgramDTO);
                trainingProgram.Id = Guid.NewGuid();
                trainingProgram.Status = "Active";
                await _unitOfWork.TrainingProgramRepository.AddAsync(trainingProgram);
                await _unitOfWork.SaveChangeAsync();
               
                // Create Training Program Detail (Syllabuses - TrainingProgram)
                foreach (var syllabusId in syllabusesId)
                {
                    var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(syllabusId);
                    if (syllabus is not null)
                    {
                        var newDetailProgramSyllabus = new DetailTrainingProgramSyllabus { SyllabusId = syllabus.Id, TrainingProgramId = trainingProgram.Id, Status = "active" };
                        await _unitOfWork.DetailTrainingProgramSyllabusRepository.AddAsync(newDetailProgramSyllabus);
                    }
                    trainingProgram.Duration += syllabus!.Duration;
                }
                _unitOfWork.TrainingProgramRepository.Update(trainingProgram);
                if (await _unitOfWork.SaveChangeAsync() > 0) return trainingProgram;
            }
            return null;

        }

        public async Task<bool> UpdateTrainingProgram(UpdateTrainingProgramDTO updateProgramDTO)
        {
            var updateProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(updateProgramDTO.Id.Value);
            if (updateProgram is not null)
            {
                updateProgram = _mapper.Map<TrainingProgram>(updateProgramDTO);
                updateProgram.Status = "Active";
                if (updateProgram is not null) _unitOfWork.TrainingProgramRepository.Update(updateProgram);
                var detailProgramSyllbuses = await _unitOfWork.DetailTrainingProgramSyllabusRepository.FindAsync(x => x.TrainingProgramId == updateProgram.Id);
                if (detailProgramSyllbuses is not null) _unitOfWork.DetailTrainingProgramSyllabusRepository.SoftRemoveRange(detailProgramSyllbuses);

                var syllabusesId = updateProgramDTO.SyllabusesId;
                foreach (var syllabusId in syllabusesId)
                {
                    var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(syllabusId);
                    if (syllabus is not null)
                    {
                        var newDetailProgramSyllabus = new DetailTrainingProgramSyllabus { SyllabusId = syllabus.Id, TrainingProgramId = updateProgramDTO.Id.Value, Status = "active" };
                        await _unitOfWork.DetailTrainingProgramSyllabusRepository.AddAsync(newDetailProgramSyllabus);
                        updateProgram!.Duration += syllabus.Duration;
                    }
                }
                _unitOfWork.TrainingProgramRepository.Update(updateProgram!);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                    return true;
            }
            return false;

        }

        public async Task<bool> DeleteTrainingProgram(Guid trainingProgramId)
        {
            var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(trainingProgramId);
            if (trainingProgram is not null)
            {
                _unitOfWork.TrainingProgramRepository.SoftRemove(trainingProgram);

                var detailProgramSyllabuses = await _unitOfWork.DetailTrainingProgramSyllabusRepository.FindAsync(x => x.TrainingProgramId == trainingProgram.Id);
                if (detailProgramSyllabuses is not null)
                {
                    _unitOfWork.DetailTrainingProgramSyllabusRepository.SoftRemoveRange(detailProgramSyllabuses);
                }
                if (await _unitOfWork.SaveChangeAsync() > 0) return true;
            }
            return false;
        }



        public async Task<IEnumerable<ViewAllTrainingProgramDTO>> viewAllTrainingProgramDTOs()
        {
            var allTrainingProgram = await _unitOfWork.TrainingProgramRepository.GetAllAsync();
            var mapViewTrainingProgram = _mapper.Map<List<ViewAllTrainingProgramDTO>>(allTrainingProgram);
            var listAll = from a in mapViewTrainingProgram
                          select new
                          {
                              Id = a.Id,
                          };
            IList<ViewAllTrainingProgramDTO> viewAllTraining = new List<ViewAllTrainingProgramDTO>();
            foreach (var a in listAll)
            {
                var result = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(a.Id);
                if (result is not null && result.IsDeleted == false)
                {
                    //viet ham lay syllabusid by trainingprogramid
                    var trainingProgramView = _mapper.Map<ViewAllTrainingProgramDTO>(result);
                    trainingProgramView.Content = (ICollection<Syllabus>?)await _unitOfWork.SyllabusRepository.GetSyllabusByTrainingProgramId(trainingProgramView.Id);
                    viewAllTraining.Add(trainingProgramView);
                }
            }
            return viewAllTraining;


        }
        public async Task<List<TrainingProgramViewModel>> SearchTrainingProgram(TrainingProgramSearchFilterModels.SearchTrainingProgramModel searchTrainingProgramModel)
        {
            //init filter
            var listTP = await _unitOfWork.TrainingProgramRepository.FindAsync(u => u.IsDeleted == false);
            var listUsers = await _unitOfWork.UserRepository.GetAllAsync();
            if (listUsers.Any() && listTP.Any())
            {
                if (!searchTrainingProgramModel.Keyword.IsNullOrEmpty())
                {
                    var searchList = new List<TrainingProgram>();
                    foreach (var item in listTP)
                    {
                        if(item.ProgramName.ToLower().Contains(searchTrainingProgramModel.Keyword.ToLower()))
                        {
                            searchList.Add(item);
                        }
                    }
                    var result = _mapper.Map<List<TrainingProgramViewModel>>(searchList);
                    return LinkingName(result,listUsers);
                }
                //not inputting required field
                else
                {
                    var result = _mapper.Map<List<TrainingProgramViewModel>>(listTP);
                    return LinkingName(result,listUsers);
                }
            }
            return new List<TrainingProgramViewModel>();
        }
        public async Task<List<TrainingProgramViewModel>> FilterTrainingProgram(TrainingProgramSearchFilterModels.FilterTrainingProgramModel filterTrainingProgramModel)
        {
            bool check = filterTrainingProgramModel.CreateBy.Equals("");
            bool check2 = filterTrainingProgramModel.Status.Equals("");
            var listTP = await _unitOfWork.TrainingProgramRepository.FindAsync(tp => tp.IsDeleted == false);
            var listUsers = await _unitOfWork.UserRepository.FindAsync(u => u.IsDeleted == false);
            if (!listUsers.IsNullOrEmpty() && !listTP.IsNullOrEmpty())
            {
                if (check == false || check2 == false)
                {
                    ICriterias<TrainingProgram> statusCriteria = new StatusCriteria(filterTrainingProgramModel.Status);
                    ICriterias<TrainingProgram> createByCriteria = new CreatedByCriteria(listUsers,filterTrainingProgramModel.CreateBy);
                    ICriterias<TrainingProgram> andCriteria = new AndTrainingProgramCriteria(statusCriteria, createByCriteria);
                    var result = _mapper.Map<List<TrainingProgramViewModel>>(andCriteria.MeetCriteria(listTP));
                    return LinkingName(result, listUsers);
                }
                //not inputting required fields
                else
                {
                    var result = _mapper.Map<List<TrainingProgramViewModel>>(listTP);
                    return LinkingName(result, listUsers);
                }
            }
            else
                return new List<TrainingProgramViewModel>();
        }
        //Avoiding boilerplate code for Search and Filter function
        private protected List<TrainingProgramViewModel> LinkingName(List<TrainingProgramViewModel> listTPs, List<User> listUsers)
        {
            foreach (var tp in listTPs)
                foreach (var u in listUsers)
                    if (tp.CreatedBy.Equals(u.Id))
                    {
                        tp.CreateByUserName = u.UserName;
                        tp.CreateByUserFullname = u.FullName;
                    }
            return listTPs;
        }
        public async Task<TrainingProgram> DuplicateTrainingProgram(Guid TrainingProgramId)
        {
            var duplicateItem = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProgramId, x => x.DetailTrainingProgramSyllabus);
            if(duplicateItem is not null) 
            {
                var createItem = new TrainingProgram 
                {
                    Id = Guid.NewGuid(),
                    ProgramName = duplicateItem.ProgramName,
                    Status = "Active"
                };
                await _unitOfWork.TrainingProgramRepository.AddAsync(createItem);
                if(await _unitOfWork.SaveChangeAsync() > 0) 
                {
                    List<DetailTrainingProgramSyllabus> createdDetail = new List<DetailTrainingProgramSyllabus>();
                    foreach(var item in duplicateItem.DetailTrainingProgramSyllabus) 
                    {
                        createdDetail.Add(new DetailTrainingProgramSyllabus{   TrainingProgramId = createItem.Id,
                                                                                     SyllabusId = item.SyllabusId,
                                                                                     Status = "Active"
                                                                                 });
                    }
                    await _unitOfWork.DetailTrainingProgramSyllabusRepository.AddRangeAsync(createdDetail);
                    if(await _unitOfWork.SaveChangeAsync() > 0) return createItem;
                    else throw new Exception("Can not Insert DetailTrainingProgramSyllabuses!");
                } else throw new Exception("Add Training Program Failed _ Save Change Failed!");

            } else throw new Exception("Not found or TrainingProgram has been deleted");
        
        }
    }
}
