using Shared.DataTransferObjects;

namespace Service.Constracts
{
    public interface IUserService {
        IEnumerable<UserDTO> GetUsers(Guid companyId, bool trackChanges);
        UserDTO GetUser(Guid companyId, Guid id, bool trackChanges);
    }

}
