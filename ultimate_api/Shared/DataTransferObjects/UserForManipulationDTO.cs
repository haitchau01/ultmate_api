using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public abstract record class UserForManipulationDTO
    {
        [Required(ErrorMessage = "FirstName is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the FirstName is 30 characters.")]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "LastName is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the LastName is 30 characters.")]
        public string LastName { get; init; }

        public string PhoneNumber { get; init; }

        [Required(ErrorMessage = "DateOfBirth is a required field.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is a required field.")]
        public string Address { get; init; }

        [Required(ErrorMessage = "Email is a required field.")]
        [EmailAddress]
        public string Email { get; init; }

        public string Country { get; init; }

        public string Department { get; init; }

    }
}

