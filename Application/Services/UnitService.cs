using Application;
using Application.Interfaces;
using Application.ViewModels.SyllabusModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Unit>> GetSyllabusDetail(Guid syllabusID)
        {

            var listUnit = await _unitOfWork.UnitRepository.GetAllAsync();
            if (listUnit is null)
            {
                throw new Exception("Not Found");
            }
            return listUnit;


        }

        public async Task<Unit> AddNewUnit(UnitDTO unitDTO, Syllabus syllabus)
        {
            var NewUnit = new Unit()
            {
                Id = Guid.NewGuid(),
                UnitName = unitDTO.UnitName,
                TotalTime = unitDTO.TotalTime,
                Session = unitDTO.Session,
                IsDeleted = false,
                Syllabus = syllabus
            };
            //throw new Exception();
            await _unitOfWork.UnitRepository.AddAsync(NewUnit);
            return NewUnit;
        }
    }
}
