namespace Constracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IUserRepository User { get; }
        void Save();
    }
}
