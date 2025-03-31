using Constracts;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace ultimate_api.Utility
{
    public class UserLinks : IUserLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<UserDTO> _dataShaper;
        public UserLinks(LinkGenerator linkGenerator, IDataShaper<UserDTO> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<UserDTO> usersDto, string fields, Guid companyId, HttpContext httpContext)
        {
            var shapedUsers = ShapeData(usersDto, fields);
            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedUsers(usersDto, fields, companyId, httpContext,
               shapedUsers);
            return ReturnShapedUsers(shapedUsers);
        }

        private List<Entity> ShapeData(IEnumerable<UserDTO> usersDto, string fields) => _dataShaper.ShapeData(usersDto, fields).Select(e => e.Entity).ToList();
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
           StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedUsers(List<Entity> shapedUsers) =>
         new LinkResponse { ShapedEntities = shapedUsers };
        private LinkResponse ReturnLinkdedUsers(IEnumerable<UserDTO> usersDto, string fields, Guid companyId, HttpContext httpContext, List<Entity> shapedUsers)
        {
            var userDtoList = usersDto.ToList();
            for (var index = 0; index < userDtoList.Count(); index++)
            {
                var userLinks = CreateLinksForUser(httpContext, companyId, userDtoList[index].Id, fields);
                shapedUsers[index].Add("Links", userLinks);
            }
            var userCollection = new LinkCollectionWrapper<Entity>(shapedUsers);
            var linkedUsers = CreateLinksForUsers(httpContext, userCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = linkedUsers };
        }
        private List<Link> CreateLinksForUser(HttpContext httpContext, Guid companyId, Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                 new Link(_linkGenerator.GetUriByAction(httpContext, "GetUserForCompany", values: new { companyId, id, fields }), "self", "GET"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteUseForCompany", values: new { companyId, id }), "delete_user", "DELETE"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateUseForCompany", values: new { companyId, id }), "update_user", "PUT"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateUseForCompany", values: new { companyId, id }), "partially_update_user", "PATCH")
             };
            return links;
        }
        private LinkCollectionWrapper<Entity> CreateLinksForUsers(HttpContext httpContext, LinkCollectionWrapper<Entity> usersWrapper)
        {
            usersWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetUsersForCompany", values: new { }), "self", "GET"));
            return usersWrapper;
        }
    }
}
