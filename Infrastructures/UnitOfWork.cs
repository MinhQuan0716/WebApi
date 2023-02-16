using Application;
using Application.Repositories;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly ISyllabusRepository _syllabusRepository;
        private readonly IUnitRepository _unitRepository;

        public UnitOfWork(AppDbContext dbContext,
            IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public ISyllabusRepository SyllabusRepository => _syllabusRepository;

        public IUnitRepository UnitRepository => _unitRepository;
        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
