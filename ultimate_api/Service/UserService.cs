using AutoMapper;
using Constracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Constracts;
using Shared.DataTransferObjects;

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
        public IEnumerable<UserDTO> GetUsers(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var userFromDb = _repository.User.GetUsers(companyId, trackChanges);
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(userFromDb);
            return userDTOs;
        }
        public UserDTO GetUser(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);

            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var userDb = _repository.User.GetUser(companyId, id, trackChanges);

            if (userDb is null)
            {
                throw new UserNotFoundException(id);
            }

            var user = _mapper.Map<UserDTO>(userDb);

            return user;
        }

        public UserDTO CreateUserForCompany(Guid companyId, UserForCreationDTO userForCreation, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var userEntity = _mapper.Map<User>(userForCreation);

            _repository.User.CreateUserForCompany(companyId, userEntity);
            _repository.Save();

            var userToReturn = _mapper.Map<UserDTO>(userEntity);

            return userToReturn;
        }
        public void DeleteUserForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);

            var userForCompany = _repository.User.GetUser(companyId, id, trackChanges);
            if (userForCompany is null)
                throw new UserNotFoundException(id);

            _repository.User.DeleteUser(userForCompany);
            _repository.Save();
        }

        public void UpdateUserForCompany(Guid companyId, Guid id, UserForUpdateDTO userForUpdateDTO, bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, compTrackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);

            var userEntity = _repository.User.GetUser(companyId, id, empTrackChanges);
            if (userEntity is null)
                throw new UserNotFoundException(id);

            _mapper.Map(userForUpdateDTO, userEntity);
            _repository.Save();
        }

        public (UserForUpdateDTO userToPatch, User userEntity) GetUserForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, compTrackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);
            var userEntity = _repository.User.GetUser(companyId, id, empTrackChanges);
            if (userEntity is null) throw new UserNotFoundException(companyId);

            var userToPatch = _mapper.Map<UserForUpdateDTO>(userEntity);
            return (userToPatch, userEntity);
        }

        public void SaveChangesForPatch(UserForUpdateDTO userToPatch, User userEntity)
        {
            _mapper.Map(userToPatch, userEntity);
            _repository.Save();
        }

    }
}
