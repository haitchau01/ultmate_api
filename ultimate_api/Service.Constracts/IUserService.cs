using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Constracts
{
    public interface IUserService {
        IEnumerable<UserDTO> GetUsers(Guid companyId, bool trackChanges);

        UserDTO GetUser(Guid companyId, Guid id, bool trackChanges);

        UserDTO CreateUserForCompany(Guid companyId, UserForCreationDTO employeeForCreation, bool trackChanges);

        void DeleteUserForCompany(Guid companyId, Guid id, bool trackChanges);

        void UpdateUserForCompany(Guid companyId, Guid id, UserForUpdateDTO userForUpdate, bool compTrackChanges, bool empTrackChanges);

        (UserForUpdateDTO userToPatch, User userEntity) GetUserForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        void SaveChangesForPatch(UserForUpdateDTO userToPatch, User userEntity);

    }

}
