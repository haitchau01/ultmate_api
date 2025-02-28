﻿using AutoMapper;
using Constracts;
using Service.Constracts;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;

        private readonly Lazy<IUserService> _employeeService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, logger, mapper));
            _employeeService = new Lazy<IUserService>(() => new UserService(repositoryManager, logger, mapper));
        }

        public ICompanyService CompanyService => _companyService.Value;

        public IUserService UserService => _employeeService.Value;
    }
}
