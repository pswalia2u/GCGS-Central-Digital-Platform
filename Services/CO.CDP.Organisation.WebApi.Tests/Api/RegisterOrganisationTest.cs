using CO.CDP.Organisation.WebApi.Model;
using CO.CDP.Organisation.WebApi.Tests.Api.WebApp;
using CO.CDP.Organisation.WebApi.UseCase;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;
using static System.Net.HttpStatusCode;

namespace CO.CDP.Organisation.WebApi.Tests.Api;
public class RegisterOrganisationTest
{
    private readonly HttpClient _httpClient;
    private readonly Mock<IUseCase<RegisterOrganisation, Model.Organisation>> _registerOrganisationUseCase = new();

    public RegisterOrganisationTest()
    {
        TestWebApplicationFactory<Program> factory = new(services =>
        {
            services.AddScoped<IUseCase<RegisterOrganisation, Model.Organisation>>(_ => _registerOrganisationUseCase.Object);
        });
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task ItRegistersNewOrganisation()
    {
        var command = GivenRegisterOrganisationCommand();
        var organisation = new Model.Organisation
        {
            Id = Guid.NewGuid(),
            Name = "TheOrganisation",
            Identifier = command.Identifier,
            AdditionalIdentifiers = command.AdditionalIdentifiers,
            Address = command.Address,
            ContactPoint = command.ContactPoint,
            Roles = command.Roles
        };

        _registerOrganisationUseCase.Setup(useCase => useCase.Execute(It.IsAny<RegisterOrganisation>()))
                                    .ReturnsAsync(organisation);

        var response = await _httpClient.PostAsJsonAsync("/organisations", command);

        response.Should().HaveStatusCode(Created, await response.Content.ReadAsStringAsync());

        var returnedOrganisation = await response.Content.ReadFromJsonAsync<Model.Organisation>();
        returnedOrganisation.Should().BeEquivalentTo(organisation, options => options.ComparingByMembers<Model.Organisation>());
    }

    [Fact]
    public async Task ItHandlesOrganisationCreationFailure()
    {
        var command = GivenRegisterOrganisationCommand();

        _registerOrganisationUseCase.Setup(useCase => useCase.Execute(It.IsAny<RegisterOrganisation>()))
                                    .ReturnsAsync((Model.Organisation)null!);

        var response = await _httpClient.PostAsJsonAsync("/organisations", command);

        response.Should().HaveStatusCode(HttpStatusCode.InternalServerError, await response.Content.ReadAsStringAsync());
    }

    private static RegisterOrganisation GivenRegisterOrganisationCommand()
    {
        return new RegisterOrganisation
        {
            Name = "TheOrganisation",
            Identifier = new OrganisationIdentifier
            {
                Scheme = "ISO9001",
                Id = "1",
                LegalName = "OfficialOrganisationName",
                Uri = "http://example.org"
            },
            AdditionalIdentifiers = new List<OrganisationIdentifier>
            {
                new OrganisationIdentifier
                {
                    Scheme = "ISO14001",
                    Id = "2",
                    LegalName = "AnotherOrganisationName",
                    Uri = "http://example.com"
                }
            },
            Address = new OrganisationAddress
            {
                StreetAddress = "1234 Example St",
                Locality = "Example City",
                Region = "Example Region",
                PostalCode = "12345",
                CountryName = "Exampleland"
            },
            ContactPoint = new OrganisationContactPoint
            {
                Name = "Contact Name",
                Email = "contact@example.org",
                Telephone = "123-456-7890",
                FaxNumber = "123-456-7891",
                Url = "http://example.org/contact"
            },
            Roles = new List<int> { 1 }
        };
    }
}