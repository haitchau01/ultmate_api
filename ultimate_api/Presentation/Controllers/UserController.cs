using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Constracts;
using Shared.DataTransferObjects;
using Shared.Parameters;
using System.Text.Json;

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

        [HttpGet]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetUsersForCompany(Guid companyId, [FromQuery] UserParameters userParameters)
        {
            var linkParams = new LinkParameters(userParameters, HttpContext);
            var result = await _serviceManager.UserService.GetUsersAsync(companyId, linkParams, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);
        }

        [HttpGet("{id:guid}", Name = "GetUserForCompany")]
        public async Task<IActionResult> GetUserForCompany(Guid companyId, Guid id)
        {
            var user = await _serviceManager.UserService.GetUserAsync(companyId, id, trackChanges: false);
            return Ok(user);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateUserForCompany(Guid companyId, [FromBody] UserForCreationDTO user)
        {
            if (user is null)
                return BadRequest("UserForCreationDTO object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            var userToReturn = await _serviceManager.UserService.CreateUserForCompanyAsync(companyId, user, trackChanges: false);


            return CreatedAtRoute("GetUserForCompany", new { companyId, id = userToReturn.Id }, userToReturn);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserForCompany(Guid companyId, Guid id)
        {
            await _serviceManager.UserService.DeleteUserForCompanyAsync(companyId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUserForCompany(Guid companyId, Guid id, [FromBody] UserForUpdateDTO user)
        {
            if (user is null)
                return BadRequest("UserForUpdateDTO object is null");
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _serviceManager.UserService.UpdateUserForCompanyAsync(companyId, id, user, compTrackChanges: false, empTrackChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateUserForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<UserForUpdateDTO> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = await _serviceManager.UserService.GetUserForPatchAsync(companyId, id, compTrackChanges: false, empTrackChanges: true);

            patchDoc.ApplyTo(result.userToPatch, ModelState);

            TryValidateModel(result.userToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _serviceManager.UserService.SaveChangesForPatchAsync(result.userToPatch, result.userEntity);

            return NoContent();
        }
    }
}
