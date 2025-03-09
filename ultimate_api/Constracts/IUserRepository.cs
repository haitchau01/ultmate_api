using Entities.Models;

namespace Constracts
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers(Guid companyId, bool trackChanges);
        User GetUser(Guid companyId, Guid id, bool trackChanges);
        void CreateUserForCompany(Guid companyId, User user);
        void DeleteUser(User user);
    }
}
