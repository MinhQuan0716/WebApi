using Application;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures
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
    }
}
