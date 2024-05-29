using CO.CDP.OrganisationInformation;
using CO.CDP.OrganisationInformation.Persistence;
using CO.CDP.TestKit.Mvc;
using Xunit.Abstractions;

namespace CO.CDP.Organisation.WebApiClient.Tests;

public class OrganisationClientIntegrationTest(ITestOutputHelper testOutputHelper)
{
    private readonly TestWebApplicationFactory<Program> _factory = new(builder =>
    {
        builder.ConfigureInMemoryDbContext<OrganisationInformationContext>();
        builder.ConfigureLogging(testOutputHelper);
    });

    [Fact]
    public async Task ItTalksToTheOrganisationApi()
    {
        IOrganisationClient client = new OrganisationClient("https://localhost", _factory.CreateClient());

        var unknownPersonId = Guid.NewGuid();
        var identifier = new OrganisationIdentifier(
            scheme: "ISO9001",
            id: "1234567",
            legalName: "New Org Legal Name"
        );
        var address = new OrganisationAddress(
            streetAddress: "1234 New St",
            streetAddress2: "",
            locality: "New City",
            region: "W.Yorkshire",
            postalCode: "123456",
            countryName: "Newland"
        );
        var contactPoint = new OrganisationContactPoint(
            name: "Main Contact",
            email: "contact@neworg.com",
            telephone: "123-456-7890",
            url: "http://contact.neworg.com"
        );
        var additionalIdentifiers = new List<OrganisationIdentifier>
        {
            new(
                scheme: "ISO14001",
                id: "1234567",
                legalName: "Additional Legal Name"
            )
        };
        var roles = new List<PartyRole> { PartyRole.Supplier };
        var newOrganisation = new NewOrganisation(
            personId: unknownPersonId,
            additionalIdentifiers: additionalIdentifiers,
            address: address,
            contactPoint: contactPoint,
            identifier: identifier,
            name: "New Organisation",
            roles: roles
        );

        await Assert.ThrowsAsync<ApiException<ProblemDetails>>(() => client.CreateOrganisationAsync(newOrganisation));
    }
}