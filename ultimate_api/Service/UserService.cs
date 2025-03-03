using AutoMapper;
using Constracts;
using Entities.Exceptions;
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

            var userFromDb = _repository.User.GetEmployees(companyId, trackChanges);
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

            var employeeDb = _repository.User.GetUser(companyId, id, trackChanges);
            
            if (employeeDb is null)
            {
                throw new UserNotFoundException(id);
            }

            var employee = _mapper.Map<UserDTO>(employeeDb);
            
            return employee;
        }

    }
}
