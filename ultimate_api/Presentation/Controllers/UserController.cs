using Microsoft.AspNetCore.Mvc;
using Service.Constracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/companies/{companyId}/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UserController(IServiceManager service)
        {
            _serviceManager = service;
        }

        [HttpGet("{id:guid}", Name = "GetUserForCompany")]
        public IActionResult GetUserForCompany(Guid companyId, Guid id)
        {
            var user = _serviceManager.UserService.GetUser(companyId, id, trackChanges: false);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUserForCompany(Guid companyId, [FromBody] UserForCreationDTO employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForCreationDto object is null");

            var userToReturn = _serviceManager.UserService.CreateUserForCompany(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetUserForCompany", new { companyId, id = userToReturn.Id }, userToReturn);
        }
    }
}
