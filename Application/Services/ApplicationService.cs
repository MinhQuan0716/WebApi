using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.ApplicationViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{

    public class ApplicationService : IApplicationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppConfiguration _configuration;
        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<bool> UpdateStatus(Guid id,bool status)
        {
           var Tset=await _unitOfWork.ApplicationReapository.GetByIdAsync(id);
            if (Tset != null)
            {
                Tset.Approved = status;
                 _unitOfWork.ApplicationReapository.Update(Tset);
                await _unitOfWork.SaveChangeAsync();
                return true;

            }
            return false;
        }

       public  async Task CreateApplication(ApplicationDTO applicationDTO)
        {
           
            var Test = _mapper.Map<Applications>(applicationDTO);
           await _unitOfWork.ApplicationReapository.AddAsync(Test);
            await _unitOfWork.SaveChangeAsync();
            
        }
    }
}
