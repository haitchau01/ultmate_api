using Shared.DataTransferObjects;

namespace Service.Constracts
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDTO> GetAllCompanies(bool trackChanges);
    }
}
