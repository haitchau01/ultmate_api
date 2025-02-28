using AutoMapper;
using Constracts;
using Entities.Models;
using Service.Constracts;
using Shared.DataTransferObjects;

namespace Service
{
    sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;

        private readonly ILoggerManager _logger;

        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges)
        {
            try
            {
                var companies = _repository.Company.GetAllCompanies(trackChanges);
                var companiesDto = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
                return companiesDto;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} servicemethod {ex}");
                throw;
            }
        }

    }
}
