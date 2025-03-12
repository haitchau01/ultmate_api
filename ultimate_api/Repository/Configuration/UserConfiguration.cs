using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
            new User
            {
                Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                FirstName = "Sam",
                LastName = "Raiden",
                DateOfBirth = new DateTime(2003, 3, 12, 0, 0, 0, DateTimeKind.Utc),
                PhoneNumber = "0347347343",
                Address = "HO CHI MINH CITY",
                Country = "VIET NAME",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            },
            new User
            {
                Id = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"),
                FirstName = "Deo",
                LastName = "Rita",
                DateOfBirth = new DateTime(2002, 3, 12, 0, 0, 0, DateTimeKind.Utc),
                PhoneNumber = "0938466233",
                Address = "QUANG NINH CITY",
                Country = "VIET NAME",
                CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
            });
        }

    }
}
