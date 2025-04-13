using Marvin.Cache.Headers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Constracts;
using Shared.DataTransferObjects;
using Application.Queries;
using Application.Commands;
using Application.Notifications;
namespace Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ISender _sender;
        private readonly IPublisher _publisher;
        public CompaniesController(ISender sender, IPublisher publisher)
        {
            _sender = sender;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false));
            return Ok(companies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDTO companyForCreationDto)
        {
            if (companyForCreationDto is null)
                return BadRequest("CompanyForCreationDto object is null");
            var company = await _sender.Send(new
            CreateCompanyCommand(companyForCreationDto));
            return CreatedAtRoute("CompanyById", new { id = company.Id }, company);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, CompanyForUpdateDTO companyForUpdateDto)
        {
            if (companyForUpdateDto is null)
                return BadRequest("CompanyForUpdateDto object is null");
            await _sender.Send(new UpdateCompanyCommand(id, companyForUpdateDto,
            TrackChanges: true));
            return NoContent();
        }

        [HttpDelete("{id:guid}")] 
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _publisher.Publish(new CompanyDeletedNotification(id, TrackChanges: false));
            return NoContent();
        }

        //[HttpGet(Name = "GetCompanies")]
        //[Authorize]
        //[ResponseCache(CacheProfileName = "120SecondsDuration")]
        //public async Task<IActionResult> GetCompanies()
        //{
        //    var companies = await _serviceManager.CompanyService.GetAllCompaniesAsync(trackChanges: false);
        //    return Ok(companies);
        //}

        [HttpGet("{id}", Name = "CompanyById")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _serviceManager.CompanyService.GetCompanyAsync(id, trackChanges: false);
            return Ok(company);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _serviceManager.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(companies);
        }

        //[HttpPost(Name = "CreateCompany")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDTO company)
        //{
        //    if (company is null)
        //        return BadRequest("CompanyForCreationDTO object is null");
        //    var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(company);
        //    return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
        //    createdCompany);
        //}

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDTO> companyCollection)
        {
            var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        //[HttpDelete("{id:guid}")]
        //public async Task<IActionResult> DeleteCompany(Guid id)
        //{
        //    await _serviceManager.CompanyService.DeleteCompanyAsync(id, trackChanges: false);

        //    return NoContent();
        //}

        //[HttpPut("{id:guid}")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDTO company)
        //{
        //    if (company is null)
        //        return BadRequest("CompanyForUpdateDTO   object is null");
        //    await _serviceManager.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);

        //    return NoContent();
        //}

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

    }
}
