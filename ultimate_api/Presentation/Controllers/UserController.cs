using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Constracts;
using Shared.DataTransferObjects;

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
                return BadRequest("UserForCreationDTO object is null");

            var userToReturn = _serviceManager.UserService.CreateUserForCompany(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetUserForCompany", new { companyId, id = userToReturn.Id }, userToReturn);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteUserForCompany(Guid companyId, Guid id)
        {
            _serviceManager.UserService.DeleteUserForCompany(companyId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateUserForCompany(Guid companyId, Guid id, [FromBody] UserForUpdateDTO user)
        {
            if (user is null)
                return BadRequest("UserForUpdateDTO object is null");
            _serviceManager.UserService.UpdateUserForCompany(companyId, id, user, compTrackChanges: false, empTrackChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateUserForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<UserForUpdateDTO> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = _serviceManager.UserService.GetUserForPatch(companyId, id, compTrackChanges: false, empTrackChanges: true);

            patchDoc.ApplyTo(result.userToPatch);
            _serviceManager.UserService.SaveChangesForPatch(result.userToPatch, result.userEntity);

            return NoContent();
        }
    }
}
