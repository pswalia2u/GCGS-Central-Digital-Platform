using AutoMapper;
using CO.CDP.Common;
using CO.CDP.OrganisationInformation.Persistence;

namespace CO.CDP.Organisation.WebApi.UseCase;
public class LookupOrganisationUseCase(IOrganisationRepository organisationRepository, IMapper mapper) : IUseCase<string, Model.Organisation?>
{
    public async Task<Model.Organisation?> Execute(string name)
    {
        return await organisationRepository.FindByName(name)
            .AndThen(mapper.Map<Model.Organisation>);
    }
}