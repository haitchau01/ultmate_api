using Constracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class RepositoryManager: IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;

        private readonly Lazy<ICompanyRepository> _companyRepository;

        private readonly Lazy<IUserRepository> _userRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        }

        public ICompanyRepository Company => _companyRepository.Value;

        public IUserRepository User => _userRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }

    }
}
