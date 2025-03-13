using Entities.Models;

namespace Constracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(Guid companyId, bool trackChanges);
        Task<User> GetUserAsync(Guid companyId, Guid id, bool trackChanges);
        void CreateUserForCompany(Guid companyId, User user);
        void DeleteUser(User user);
    }
}
