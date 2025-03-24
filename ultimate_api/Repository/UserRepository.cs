using Constracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.Parameters;
using Shared.RequestFeatures;
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

        public async Task<PagedList<User>> GetUsersAsync(Guid companyId, UserParameters userParameters, bool trackChanges)
        {
            var users = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .FilterUsers(userParameters.MinAge, userParameters.MaxAge)
                .Search(userParameters.SearchTerm)
                .OrderBy(e => e.FirstName)
                .Sort(userParameters.OrderBy)
                .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
                .Take(userParameters.PageSize).ToListAsync();

            var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();
            return new PagedList<User>(users, count, userParameters.PageNumber, userParameters.PageSize);
        }

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
