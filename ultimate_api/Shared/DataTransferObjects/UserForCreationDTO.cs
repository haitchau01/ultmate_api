using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record UserForCreationDTO(string Firstname, string Lastname, int PhoneNumber, string Address, string Country, int Age);
}
