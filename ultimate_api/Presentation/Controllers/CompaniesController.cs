using Microsoft.AspNetCore.Mvc;
using Service.Constracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CompaniesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public IActionResult GetCompanies()
        {
            var companies = _serviceManager.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
        }
        [HttpGet("{id:guid}")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _serviceManager.CompanyService.GetCompany(id, trackChanges: false);
            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDTO company)
        {
            if (company is null)
                return BadRequest("CompanyForCreationDTO object is null");
            var createdCompany = _serviceManager.CompanyService.CreateCompany(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
        }
    }
}
