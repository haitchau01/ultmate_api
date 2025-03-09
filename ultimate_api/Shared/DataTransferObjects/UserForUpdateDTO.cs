namespace Shared.DataTransferObjects
{
    public record UserForUpdateDTO(string FirstName, string LastName, int PhoneNumber, string Address, string Country, int Age);
}
