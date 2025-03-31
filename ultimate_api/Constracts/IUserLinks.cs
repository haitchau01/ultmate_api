using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Constracts
{
    public interface IUserLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<UserDTO> usersDto, string fields, Guid companyId, HttpContext httpContext);
    }

}
