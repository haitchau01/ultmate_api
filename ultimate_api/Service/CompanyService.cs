using AutoMapper;
using Constracts;
using Entities.Exceptions;
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
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            return companiesDto;
        }
        public CompanyDTO GetCompany(Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(id, trackChanges);

            if (company is null)
            {
                throw new CompanyNotFoundException(id);
            }

            var companyDto = _mapper.Map<CompanyDTO>(company);
            return companyDto;
        }

        public CompanyDTO CreateCompany(CompanyForCreationDTO company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            _repository.Save();
            var companyToReturn = _mapper.Map<CompanyDTO>(companyEntity);
            return companyToReturn;
        }

        public IEnumerable<CompanyDTO> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = _repository.Company.GetByIds(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
            return companiesToReturn;
        }
        public (IEnumerable<CompanyDTO> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDTO> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            _repository.Save();
            var companyCollectionToReturn =
            _mapper.Map<IEnumerable<CompanyDTO>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);
        }
        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);

            _repository.Company.DeleteCompany(company);
            _repository.Save();
        }

        public void UpdateCompany(Guid companyId, CompanyForUpdateDTO companyForUpdate, bool trackChanges)
        {
            var companyEntity = _repository.Company.GetCompany(companyId, trackChanges); if (companyEntity is null)
                throw new CompanyNotFoundException(companyId);

            _mapper.Map(companyForUpdate, companyEntity);
            _repository.Save();
        }

    }
}
