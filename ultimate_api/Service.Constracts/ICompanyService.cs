using Shared.DataTransferObjects;

namespace Service.Constracts
{
    public interface ICompanyService
    {

        Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync(bool trackChanges);
        Task<CompanyDTO> GetCompanyAsync(Guid companyId, bool trackChanges);
        Task<CompanyDTO> CreateCompanyAsync(CompanyForCreationDTO company);    
        Task<IEnumerable<CompanyDTO>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<CompanyDTO> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDTO> companyCollection);
        Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
        Task UpdateCompanyAsync(Guid companyid, CompanyForUpdateDTO companyForUpdate, bool trackChanges);

    }
}
