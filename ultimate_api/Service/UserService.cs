using AutoMapper;
using Constracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Constracts;
using Shared.DataTransferObjects;
using System.Threading.Tasks;

namespace Service
{
    sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;

        private readonly ILoggerManager _logger;

        private readonly IMapper _mapper;


        public UserService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDTO>> GetUsersAsync(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var userFromDb = await _repository.User.GetUsersAsync(companyId, trackChanges);
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(userFromDb);
            return userDTOs;
        }
        public async Task<UserDTO> GetUserAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var userDb = await _repository.User.GetUserAsync(companyId, id, trackChanges);

            if (userDb is null)
            {
                throw new UserNotFoundException(id);
            }

            var user = _mapper.Map<UserDTO>(userDb);

            return user;
        }

        public async Task<UserDTO> CreateUserForCompanyAsync(Guid companyId, UserForCreationDTO userForCreation, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            userForCreation.DateOfBirth = userForCreation.DateOfBirth.ToUniversalTime();
            var userEntity = _mapper.Map<User>(userForCreation);

            _repository.User.CreateUserForCompany(companyId, userEntity);
            await _repository.SaveAsync();

            var userToReturn = _mapper.Map<UserDTO>(userEntity);

            return userToReturn;
        }
        public async Task DeleteUserForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);

            var userForCompany = await _repository.User.GetUserAsync(companyId, id, trackChanges);
            if (userForCompany is null)
                throw new UserNotFoundException(id);

            _repository.User.DeleteUser(userForCompany);
            await _repository.SaveAsync();
        }

        public async Task UpdateUserForCompanyAsync(Guid companyId, Guid id, UserForUpdateDTO userForUpdateDTO, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);

            var userEntity = await _repository.User.GetUserAsync(companyId, id, empTrackChanges);
            if (userEntity is null)
                throw new UserNotFoundException(id);

            _mapper.Map(userForUpdateDTO, userEntity);
            await _repository.SaveAsync();
        }

        public async Task<(UserForUpdateDTO userToPatch, User userEntity)> GetUserForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);
            var userEntity = await _repository.User.GetUserAsync(companyId, id, empTrackChanges);
            if (userEntity is null) throw new UserNotFoundException(companyId);

            var userToPatch = _mapper.Map<UserForUpdateDTO>(userEntity);
            return (userToPatch, userEntity);
        }

        public async Task SaveChangesForPatchAsync(UserForUpdateDTO userToPatch, User userEntity)
        {
            _mapper.Map(userToPatch, userEntity);
            await _repository.SaveAsync();
        }

    }
}
