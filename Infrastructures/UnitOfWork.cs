using Application;
using Application.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly ISyllabusRepository _syllabusRepository;

        public UnitOfWork(AppDbContext dbContext,
            IUserRepository userRepository,ISyllabusRepository syllabusRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _syllabusRepository= syllabusRepository;
        }

        public IUserRepository UserRepository => _userRepository;

        public ISyllabusRepository SyllabusRepository => _syllabusRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
