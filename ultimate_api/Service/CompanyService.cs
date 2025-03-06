﻿using AutoMapper;
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
    }
}
