using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SyllabusService : ISyllabusService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IClaimsService _claimsservice;


        public SyllabusService(IUnitOfWork unitofwork, IClaimsService claimsservice)
        {
            _unitofWork = unitofwork;
            _claimsservice = claimsservice;
        }

        public async Task<List<Syllabus>> FilterSyllabus(double duration1, double duration2)
        {
            var filterSyllabusList = await _unitofWork.SyllabusRepository.FilterSyllabusByDuration(duration1, duration2);
            return filterSyllabusList;
        }

        public Task<List<Syllabus>> GetAllSyllabus()
        {
            var syllabusList = _unitofWork.SyllabusRepository.GetAllAsync();
            return syllabusList;
        }

        public async Task<bool> DeleteSyllabussAsync(string syllabusID)
        {
            bool check = false;
            var syllabusFind = await _unitofWork.SyllabusRepository.GetByIdAsync(Guid.Parse(syllabusID));
            if (syllabusFind is not null && syllabusFind.IsDeleted == false)
            {
                syllabusFind.DeletionDate = DateTime.Now;
                syllabusFind.DeleteBy = _claimsservice.GetCurrentUserId;
                syllabusFind.IsDeleted = true;
                check = true;
            }
            await _unitofWork.SaveChangeAsync();
            return check;


        }
    }
}



     
    
    
