using Constracts;

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

        public void Save() => _repositoryContext.SaveChanges();

    }
}
