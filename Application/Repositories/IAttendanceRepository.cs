using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IAttendanceRepository:IGenericRepository<Attendance>
    {
        public List<Attendance> GetAttendancesByTraineeClassID(Guid id);
        public List<Attendance> GetAttendancesByTraineeID(Guid id);
    }
    
}
