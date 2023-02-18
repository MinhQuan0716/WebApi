using Application.Commons;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
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
        


        public SyllabusService(IUnitOfWork unitofwork, IClaimsService claimsservice, IUnitOfWork unitOfWork, IMapper mapper,AppConfiguration configuration )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;    
            _claimsservice = claimsservice;
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
    }
}



     
    
    
