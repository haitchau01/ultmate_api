using Entities.Models;
using Shared.DataTransferObjects;
using Shared.Parameters;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service.Constracts
{
    public interface IUserService
    {

        Task<(IEnumerable<ExpandoObject> users, MetaData metaData)> GetUsersAsync(Guid companyId, UserParameters userParameters, bool trackChanges);

        Task<IEnumerable<UserDTO>> GetUsersAsync(Guid companyId, bool trackChanges);

        Task<UserDTO> GetUserAsync(Guid companyId, Guid id, bool trackChanges);

        Task<UserDTO> CreateUserForCompanyAsync(Guid companyId, UserForCreationDTO userForCreation, bool trackChanges);

        Task DeleteUserForCompanyAsync(Guid companyId, Guid id, bool trackChanges);

        Task UpdateUserForCompanyAsync(Guid companyId, Guid id, UserForUpdateDTO userForUpdate, bool compTrackChanges, bool empTrackChanges);

        Task<(UserForUpdateDTO userToPatch, User userEntity)> GetUserForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        Task SaveChangesForPatchAsync(UserForUpdateDTO userToPatch, User userEntity);

    }

}
