using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.Constracts;

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

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _serviceManager.CompanyService.GetAllCompanies(trackChanges: false);
            if (companies == null)
            {
                throw new NotFoundException("Company information not found!");
            }
            return Ok(companies);
        }
    }
}
