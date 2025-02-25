using Constracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ultimate_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        public UserController(ILoggerManager logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var newListString = new List<string>() { "hehe"};
            _logger.LogInfo("Here is info message from our values controller");

            return newListString;
        }
    }
}
