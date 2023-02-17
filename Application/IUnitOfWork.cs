using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public ISyllabusRepository SyllabusRepository { get; }
        
        public IUnitRepository UnitRepository { get; }
        public IUserRepository UserRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
