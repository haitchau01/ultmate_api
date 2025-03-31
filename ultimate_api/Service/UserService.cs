using AutoMapper;
using Constracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Service.Constracts;
using Shared.DataTransferObjects;
using Shared.Parameters;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service
{
    sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;

        private readonly ILoggerManager _logger;

        private readonly IMapper _mapper;

        private readonly IDataShaper<UserDTO> _dataShaper;
        private readonly IUserLinks _userLinks;


        public UserService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IUserLinks userLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userLinks = userLinks;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync(Guid companyId, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var userFromDb = await _repository.User.GetUsersAsync(companyId, trackChanges);
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(userFromDb);
            return userDTOs;
        }
        public async Task<UserDTO> GetUserAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

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
            await CheckIfCompanyExists(companyId, trackChanges);

            userForCreation.DateOfBirth = userForCreation.DateOfBirth.ToUniversalTime();
            var userEntity = _mapper.Map<User>(userForCreation);

            _repository.User.CreateUserForCompany(companyId, userEntity);
            await _repository.SaveAsync();

            var userToReturn = _mapper.Map<UserDTO>(userEntity);

            return userToReturn;
        }
        public async Task DeleteUserForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);

            var userForCompany = await GetUserForCompanyAndCheckIfItExists(companyId, id, trackChanges);

            _repository.User.DeleteUser(userForCompany);
            await _repository.SaveAsync();
        }

        public async Task UpdateUserForCompanyAsync(Guid companyId, Guid id, UserForUpdateDTO userForUpdateDTO, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, empTrackChanges);

            var userEntity = await GetUserForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            _mapper.Map(userForUpdateDTO, userEntity);
            await _repository.SaveAsync();
        }

        public async Task<(UserForUpdateDTO userToPatch, User userEntity)> GetUserForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges); if (company is null)
                await CheckIfCompanyExists(companyId, compTrackChanges);

            var userDb = await GetUserForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            var userToPatch = _mapper.Map<UserForUpdateDTO>(userDb);

            return (userToPatch: userToPatch, userEntity: userDb);

        }

        public async Task SaveChangesForPatchAsync(UserForUpdateDTO userToPatch, User userEntity)
        {
            _mapper.Map(userToPatch, userEntity);
            await _repository.SaveAsync();
        }

        //public async Task<(IEnumerable<ShapedEntity> users, MetaData metaData)> GetUsersAsync(Guid companyId, UserParameters userParameters, bool trackChanges)
        //{
        //    if (!userParameters.ValidAgeRange)
        //        throw new MaxAgeRangeBadRequestException();
        //    await CheckIfCompanyExists(companyId, trackChanges);

        //    var usersWithMetaData = await _repository.User.GetUsersAsync(companyId, userParameters, trackChanges);

        //    var userDTO = _mapper.Map<IEnumerable<UserDTO>>(usersWithMetaData);
        //    var shapedData = _dataShaper.ShapeData(userDTO, userParameters.Fields);

        //    return (users: shapedData, metaData: usersWithMetaData.MetaData);
        //}

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges); if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task<User> GetUserForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var userDb = await _repository.User.GetUserAsync(companyId, id, trackChanges);
            if (userDb is null)
                throw new UserNotFoundException(id);
            return userDb;
        }


        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetUsersAsync(Guid companyId, LinkParameters linkParameters, bool trackChanges)
        {
            if (!linkParameters.UserParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();
            await CheckIfCompanyExists(companyId, trackChanges);
            var usersWithMetaData = await _repository.User.GetUsersAsync(companyId, linkParameters.UserParameters,
            trackChanges);
            var userDto =
            _mapper.Map<IEnumerable<UserDTO>>(usersWithMetaData);
            var links = _userLinks.TryGenerateLinks(userDto, linkParameters.UserParameters.Fields,companyId, linkParameters.Context);
            return (linkResponse: links, metaData: usersWithMetaData.MetaData);
        }
    }
}
