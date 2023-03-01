﻿using Application.Interfaces;
using Application.ViewModels.TrainingProgramModels;
using AutoMapper;
using Domain.Entities;
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
           if(result is not null && result.IsDeleted == false)
           {
                var trainingProgramView = _mapper.Map<TrainingProgramViewModel>(result);
                if(trainingProgramView.TrainingProgramId is not null)
                trainingProgramView.Syllabuses = (ICollection<Syllabus>?)await _unitOfWork.SyllabusRepository.GetSyllabusByTrainingProgramId(trainingProgramView.TrainingProgramId.Value);
                return trainingProgramView;
           }
            return null;
        }

        public async Task<TrainingProgram> CreateTrainingProgram(CreateTrainingProgramDTO createTrainingProgramDTO) 
        {
            var syllabusesId = createTrainingProgramDTO.SyllabusesId;
            if(syllabusesId is not null)
            {
                

                // Create Training Program
                var trainingProgram = _mapper.Map<TrainingProgram>(createTrainingProgramDTO);
                trainingProgram.Id = Guid.NewGuid();
                await _unitOfWork.TrainingProgramRepository.AddAsync(trainingProgram);
                await _unitOfWork.SaveChangeAsync();

                // Create Training Program Detail (Syllabuses - TrainingProgram)
                foreach (var syllabusId in syllabusesId)
                {
                    var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(syllabusId);
                    if (syllabus is not null)
                    {
                        var newDetailProgramSyllabus = new DetailTrainingProgramSyllabus { SyllabusId = syllabus.Id, TrainingProgramId = trainingProgram.Id, Status = "active"};
                        await _unitOfWork.DetailTrainingProgramSyllabusRepository.AddAsync(newDetailProgramSyllabus);
                    }
                }
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
                    }
                }

                if (await _unitOfWork.SaveChangeAsync() > 0)
                return true;
            }
            return false;

        }

        public async Task<bool> DeleteTrainingProgram(Guid trainingProgramId)
        {
            var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(trainingProgramId);
            if(trainingProgram is not null)
            {
                _unitOfWork.TrainingProgramRepository.SoftRemove(trainingProgram);

                var detailProgramSyllabuses = await _unitOfWork.DetailTrainingProgramSyllabusRepository.FindAsync(x => x.TrainingProgramId == trainingProgram.Id);
                if(detailProgramSyllabuses is not null)
                {
                    _unitOfWork.DetailTrainingProgramSyllabusRepository.SoftRemoveRange(detailProgramSyllabuses);
                }
                if (await _unitOfWork.SaveChangeAsync() > 0) return true;
            }
            return false;
        }
    }
}