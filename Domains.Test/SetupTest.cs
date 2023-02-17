using Application;
using Application.Commons;
using Application.Interfaces;
using Application.Repositories;
using Application.Utils;
using AutoFixture;
using AutoMapper;
using Infrastructures;
using Infrastructures.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        protected readonly Mock<AppConfiguration> _appConfigurationMock;
        protected readonly Mock<ISendMailHelper> _sendMailMock;
        protected readonly Mock<ISyllabusService> _syllabusServiceMock;
        protected readonly Mock<IUnitService> _unitServiceMock;
        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigurationsProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _userServiceMock = new Mock<IUserService>();
            _claimsServiceMock = new Mock<IClaimsService>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _sendMailMock = new Mock<ISendMailHelper>();
            _userRepository = new Mock<IUserRepository>();
            _unitServiceMock = new Mock<IUnitService>();
            _syllabusServiceMock = new Mock<ISyllabusService>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);

            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
            _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(Guid.Empty);
            _appConfigurationMock = new Mock<AppConfiguration>();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}