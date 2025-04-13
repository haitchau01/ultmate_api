using MediatR;
using Shared.DataTransferObjects;
namespace Application.Commands
{
    public sealed record UpdateCompanyCommand(Guid Id, CompanyForUpdateDTO Company, bool TrackChanges) : IRequest<Unit>;
}
