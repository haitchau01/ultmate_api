using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User
    {
        [Column("User_Id")]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
