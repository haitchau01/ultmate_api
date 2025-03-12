using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Country { get; set; }

        public string? Department { get; set; }

        public Guid CompanyId { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
