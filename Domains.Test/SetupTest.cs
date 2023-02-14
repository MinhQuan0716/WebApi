using Application;
using Application.Interfaces;
using Application.Repositories;
using AutoFixture;
using AutoMapper;
using Infrastructures;
using Infrastructures.Mappers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Domains.Test
{
    public class SetupTest:IDisposable
    {

        protected readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<IUserService> _userServiceMock;

        protected readonly Mock<IClaimsService> _claimsServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;

        protected readonly Mock<IUserRepository> _userRepository;
        protected readonly AppDbContext _dbContext;
        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigurationsProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _userServiceMock = new Mock<IUserService>();
            _claimsServiceMock = new Mock<IClaimsService>();
            _currentTimeMock = new Mock<ICurrentTime>();

            _userRepository = new Mock<IUserRepository>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);

            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
            _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(Guid.Empty);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}