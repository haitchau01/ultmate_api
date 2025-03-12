using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Shared.DataTransferObjects
{
    public record UserDTO
    {
        public Guid Id { get; init; }

        public string FullName { get; init; }

        public int Age { get; init; }

        public string PhoneNumber { get; init; }

        public string Address { get; init; }

        public string Email { get; init; }

        public string Country { get; init; }

        public string Department { get; init; }

    };
}
