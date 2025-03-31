using Microsoft.AspNetCore.Http;
using Shared.Parameters;

namespace Entities.Models
{
    public record LinkParameters(UserParameters UserParameters, HttpContext Context);
}
