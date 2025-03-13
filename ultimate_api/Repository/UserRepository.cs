using Constracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public void CreateUserForCompany(Guid companyId, User user)
        {
            user.CompanyId = companyId;
            Create(user);
        }

        public void DeleteUser(User user) => Delete(user);


        public async Task<IEnumerable<User>> GetUsersAsync(Guid companyId, bool trackChanges)
        {
            return await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).OrderBy(e => e.FirstName).ToListAsync();
        }

        public async Task<User?> GetUserAsync(Guid companyId, Guid id, bool trackChanges)
        {
            return await FindByCondition(usr => usr.CompanyId.Equals(companyId) && usr.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

    }
}
