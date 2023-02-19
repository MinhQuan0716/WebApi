using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Services
{
    public class SyllabusService : ISyllabusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsservice;
        private readonly IMapper _mapper;
        private readonly AppConfiguration _configuration;


        public SyllabusService(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _claimsservice = claimsService;
        }
        public SyllabusService(IUnitOfWork unitofwork, IClaimsService claimsservice, IUnitOfWork unitOfWork, IMapper mapper,AppConfiguration configuration )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;    
            _claimsservice = claimsservice;
        }

        public SyllabusService(IUnitOfWork unitofWork, IClaimsService claimsservice, IMapper mapper) : this(unitofWork, claimsservice)
        {
            _mapper = mapper;
        }

        public async Task<List<Syllabus>> FilterSyllabus(double duration1, double duration2)
        {
            var filterSyllabusList = await _unitOfWork.SyllabusRepository.FilterSyllabusByDuration(duration1, duration2);
            return filterSyllabusList;
           
        }

        public Task<List<Syllabus>> GetAllSyllabus()
        {
            var syllabusList = _unitOfWork.SyllabusRepository.GetAllAsync();
            return syllabusList;
        }
        public Task<List<Syllabus>> GetByName(string name)
        {
            
           var result=_unitOfWork.SyllabusRepository.SearchByName(name);
            return result;
        }

        public async Task<bool> DeleteSyllabussAsync(string syllabusID)
        {
            bool check = false;
            var syllabusFind = await _unitOfWork.SyllabusRepository.GetByIdAsync(Guid.Parse(syllabusID));
            if (syllabusFind is not null && syllabusFind.IsDeleted == false)
            {
                syllabusFind.DeletionDate = DateTime.Now;
                syllabusFind.DeleteBy = _claimsservice.GetCurrentUserId;
                syllabusFind.IsDeleted = true;
                check = true;
            }
            await _unitOfWork.SaveChangeAsync();
            return check;


        }

        /// <summary>
        /// Update a Syllabus
        /// </summary>
        /// <param name="syllabusId">ID of updated syllabus</param>
        /// <param name="updateItem">Changing description</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateSyllabus(Guid syllabusId, UpdateSyllabusDTO updateItem)
        {
           var result = false;
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(syllabusId);

            if (syllabus is not null)
            {
                if (_mapper is not null)
                {
                    var updatedUnitDTO = updateItem.Units.Where(u => u.UnitID is not null).ToList();

                    // Check if unitId is null or not, if null, create new unit
                    if (updateItem.Units is not null)
                    {
                        var createUnit = updateItem.Units.Where(u => u.UnitID is null).ToList();
                        foreach (var item in updateItem.Units) item.syllabusId = syllabusId;
                        if (createUnit.Count > 0)
                        {
                            foreach (var unit in createUnit)
                            {
                                unit.UnitID = Guid.NewGuid();
                            }
                            var unitList = _mapper.Map<List<Unit>>(createUnit);
                            await _unitOfWork.UnitRepository.AddRangeAsync(unitList);

                        }
                    }
                    // Section Get All Lecture, set unit Id, prepare for lecture to update
                    var updateLecture = from u in updateItem.Units
                                        select u.UpdateLectureDTOs.ToList().FirstOrDefault();
                    var lectures = await _unitOfWork.LectureRepository.GetLectureBySyllabusId(syllabusId);
                    var updateLectureList = updateLecture.ToList();
                    foreach (var unit in updateItem.Units)
                    {
                        foreach (var lec in unit.UpdateLectureDTOs)
                        {
                            lec.UnitID = unit.UnitID;
                        }
                    }
                    // Update unit which is not created and syllabus Information
                    updateItem.Units = updatedUnitDTO;
                    _mapper.Map(updateItem, syllabus, typeof(UpdateSyllabusDTO), typeof(Syllabus));
                    _unitOfWork.SyllabusRepository.Update(syllabus);
                    await _unitOfWork.SaveChangeAsync();
                   
                    // Starting Update/Create lecture
                    foreach (var item in updateLectureList)
                    {
                        if (item.LectureID is null)
                        {
                            var newID = Guid.NewGuid();
                            item.LectureID = newID;
                            var lecture = _mapper.Map<Lecture>(item);

                            await _unitOfWork.LectureRepository.AddAsync(lecture);
                            if (await _unitOfWork.SaveChangeAsync() > 0)
                            {
                                if (item.UnitID is null) return false; 
                                UpdateUnitLectureDTO detailDTO = new UpdateUnitLectureDTO
                                {
                                    UnitId = item.UnitID.Value,
                                    LectureId = newID
                                };
                                var detail = _mapper.Map<DetailUnitLecture>(detailDTO);
                                await _unitOfWork.DetailUnitLectureRepository.AddAsync(detail);
                            }
                        }
                        else
                        {
                            var lecture = lectures.FirstOrDefault(l => l.Id == item.LectureID);
                            _ = _mapper.Map(item, lecture, typeof(UpdateLectureDTO), typeof(Lecture));
                            if (lecture is not null)
                            {
                                _unitOfWork.LectureRepository.Update(lecture);
                            }

                        }
                    }
                    if (await _unitOfWork.SaveChangeAsync() > 0) result = true;
                }
            }
            return result;
        }
    }
}



     
    
    
