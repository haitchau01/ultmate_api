using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class User
    {
        [Column("User_Id")]
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int Age { get; set; }

        public int PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Country { get; set; }

        public Guid CompanyId { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
