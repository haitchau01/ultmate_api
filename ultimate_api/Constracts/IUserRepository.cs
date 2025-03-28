﻿using Entities.Models;
using Shared.Parameters;
using Shared.RequestFeatures;
using System.Threading.Tasks;

namespace Constracts
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetUsersAsync(Guid companyId, UserParameters userParameters, bool trackChanges);
        Task<IEnumerable<User>> GetUsersAsync(Guid companyId, bool trackChanges);
        Task<User> GetUserAsync(Guid companyId, Guid id, bool trackChanges);
        void CreateUserForCompany(Guid companyId, User user);
        void DeleteUser(User user);
    }
}
