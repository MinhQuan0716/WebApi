using Application.Commons;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppConfiguration _configuration;
        public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper, AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
       

        public async  Task<List<Attendance>> GetAttendanceByTraineeID(Guid id)
        {
            var finResult =  _unitOfWork.AttendanceRepository.GetAttendancesByTraineeID(id);
            return finResult;
        }

        public async Task<List<Attendance>> GetAttendancesByTraineeClassID(Guid id)
        {
            var findResult = _unitOfWork.AttendanceRepository.GetAttendancesByTraineeClassID(id);
            return findResult;
        }
    }
}
