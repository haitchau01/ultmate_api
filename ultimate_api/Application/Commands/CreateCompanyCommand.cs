using MediatR;
using Shared.DataTransferObjects;

namespace Application.Commands
{
    public sealed record CreateCompanyCommand(CompanyForCreationDTO Company) : IRequest<CompanyDTO>;
}
