﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id:guid}")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _serviceManager.CompanyService.GetCompany(id, trackChanges: false);
            return Ok(company);
        }

    }
}
