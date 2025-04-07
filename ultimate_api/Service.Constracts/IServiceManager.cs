namespace Service.Constracts
{
    public interface IServiceManager
    {
        ICompanyService CompanyService { get; }

        IUserService UserService { get; }

        IAuthenticationService AuthenticationService { get; }
    }
}
