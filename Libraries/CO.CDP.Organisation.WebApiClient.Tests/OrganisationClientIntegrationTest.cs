namespace CO.CDP.Organisation.WebApiClient.Tests;

public class OrganisationClientIntegrationTest
{
    [Fact(Skip = "The test requires the organisation service to run.")]
    public async Task ItTalksToTheOrganisationApi()
    {
        IOrganisationClient client = new OrganisationClient("http://localhost:5288", new HttpClient());

        var identifier = new OrganisationIdentifier(
            scheme: "ISO9001",
            id: "1",
            legalName: "New Org Legal Name",
            uri: "http://neworg.com"
        );
        var address = new OrganisationAddress(
            streetAddress: "1234 New St",
            locality: "New City",
            region: "New State",
            postalCode: "123456",
            countryName: "Newland"
        );
        var contactPoint = new OrganisationContactPoint(
            name: "Main Contact",
            email: "contact@neworg.com",
            telephone: "123-456-7890",
            faxNumber: "123-456-7891",
            url: "http://contact.neworg.com"
        );
        var additionalIdentifiers = new List<OrganisationIdentifier>
    {
        new OrganisationIdentifier(
            scheme: "ISO14001",
            id: "2",
            legalName: "Additional Legal Name",
            uri: "http://additionalorg.com"
        )
    };
        var roles = new List<int> { 1 };
        var newOrganisation = new NewOrganisation(
            additionalIdentifiers: additionalIdentifiers,
            address: address,
            contactPoint: contactPoint,
            identifier: identifier,
            name: "New Organisation",
            roles: roles
        );
        var organisation = await client.CreateOrganisationAsync(newOrganisation);

        var foundOrganisation = await client.GetOrganisationAsync(organisation.Id);

        Assert.Equal(organisation, foundOrganisation);
    }
}