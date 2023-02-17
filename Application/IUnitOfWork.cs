using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public ISyllabusRepository SyllabusRepository { get; }
        

        public IUserRepository UserRepository { get; }

        public IUnitRepository UnitRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
