using Entities.Models;

namespace Repository.Extensions
{
    public static class RepositoryUserExtensions
    {
        public static IQueryable<User> FilterUsers(this IQueryable<User> users, uint minAge, uint maxAge)
        {
            return users.Where(e => DateTime.UtcNow.Year - e.DateOfBirth.Value.Year >= minAge && DateTime.UtcNow.Year - e.DateOfBirth.Value.Year <= maxAge);
        }

        public static IQueryable<User> Search(this IQueryable<User> users, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return users;
            var lowerCaseTerm = searchTerm.Trim().ToLower();

            //EF.Functions.Like(u.FirstName, $"%{searchTerm}%") || EF.Functions.Like(u.LastName, $"%{searchTerm}%")
            return users.Where(e => (e.FirstName + " " + e.LastName).ToLower().Contains(lowerCaseTerm));
        }
    }
}
