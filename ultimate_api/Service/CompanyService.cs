using Constracts;
using Service.Constracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    sealed class CompanyService : ICompanyService
    {
            private readonly IRepositoryManager _repository;
            private readonly ILoggerManager _logger;
            public CompanyService(IRepositoryManager repository, ILoggerManager
            logger)
            {
                _repository = repository;
                _logger = logger;
            }
    }
}
