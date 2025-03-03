using Entities.Models;

namespace Constracts
{
    public interface IUserRepository
    {
        IEnumerable<User> GetEmployees(Guid companyId, bool trackChanges);
        User GetUser(Guid companyId, Guid id, bool trackChanges);
    }
}
