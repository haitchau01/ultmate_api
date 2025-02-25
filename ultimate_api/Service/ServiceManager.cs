using Constracts;
using Service.Constracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IUserService> _employeeService;
        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager
        logger)
        {
            _companyService = new Lazy<ICompanyService>(() => new
            CompanyService(repositoryManager, logger));
            _employeeService = new Lazy<IUserService>(() => new
            UserService(repositoryManager, logger));
        }
        public ICompanyService CompanyService => _companyService.Value;
        public IUserService UserService => _employeeService.Value;
    }
}
