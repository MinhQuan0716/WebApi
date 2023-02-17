using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        
        public IUserRepository UserRepository { get; }
        public ISyllabusRepository SyllabusRepository { get; }
        public Task<int> SaveChangeAsync();
    }
}
