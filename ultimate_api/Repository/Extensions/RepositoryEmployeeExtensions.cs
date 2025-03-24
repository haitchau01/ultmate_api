using Entities.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;

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

        public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return users.OrderBy(e => e.FirstName);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            if (string.IsNullOrWhiteSpace(orderQuery))
                return users.OrderBy(e => e.FirstName);

            return users.OrderBy(orderQuery);
        }

    }
}
