using Constracts;
using Entities.Models;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<User> GetEmployees(Guid companyId, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).OrderBy(e => e.FirstName).ToList();
        }

        public User? GetUser(Guid companyId, Guid id, bool trackChanges)
        {
            return FindByCondition(usr => usr.CompanyId.Equals(companyId) && usr.Id.Equals(id), trackChanges).SingleOrDefault();
        }
    }
}
