namespace Shared.DataTransferObjects
{
    public record UserForCreationDTO(string Firstname, string Lastname, int PhoneNumber, string Address, string Country, int Age);
}
