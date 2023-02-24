using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
   public  interface IAttendanceService
    {
        public Task<List<Attendance>> GetAttendancesByTraineeClassID(Guid id); 
        public Task<List<Attendance>> GetAttendanceByTraineeID(Guid id);
    }
}
