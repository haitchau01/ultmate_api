﻿using Shared.DataTransferObjects;

namespace Service.Constracts
{
    public interface ICompanyService
    {

        IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges);
        CompanyDTO GetCompany(Guid companyId, bool trackChanges);
        CompanyDTO CreateCompany(CompanyForCreationDTO company);    
        IEnumerable<CompanyDTO> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        (IEnumerable<CompanyDTO> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDTO> companyCollection);
        void DeleteCompany(Guid companyId, bool trackChanges);
        void UpdateCompany(Guid companyid, CompanyForUpdateDTO companyForUpdate, bool trackChanges);

    }
}
