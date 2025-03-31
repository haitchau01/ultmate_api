using AutoMapper;
using Constracts;
using Service.Constracts;
using Shared.DataTransferObjects;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;

        private readonly Lazy<IUserService> _userService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IUserLinks userLinks)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, logger, mapper));
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, logger, mapper, userLinks));
        }

        public ICompanyService CompanyService => _companyService.Value;

        public IUserService UserService => _userService.Value;
    }
}
