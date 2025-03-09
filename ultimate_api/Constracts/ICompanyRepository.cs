using Entities.Models;

namespace Constracts
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        Company GetCompany(Guid companyId, bool trackChanges);
        void CreateCompany(Company company);

        void DeleteCompany(Company company);
    }
}
