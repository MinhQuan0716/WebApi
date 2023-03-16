using Application.Repositories;
using Application.ViewModels.AtttendanceModels;
using Domain.Entities;

namespace Infrastructures.Repositories
{
    public interface IApplicationRepository : IGenericRepository<Applications>
    {
        Task<IList<Applications>> GetAllApplicationByClassAndDateTime(Guid? classId, DateTime dateTime);
        Task<Applications> GetApplicationByUserAndClassId(AttendanceDTO attendance, Guid classId);
    }
}