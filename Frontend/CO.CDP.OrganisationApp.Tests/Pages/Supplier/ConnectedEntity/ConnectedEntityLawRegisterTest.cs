using CO.CDP.Organisation.WebApiClient;
using CO.CDP.OrganisationApp.Pages.Supplier;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

namespace CO.CDP.OrganisationApp.Tests.Pages.Supplier.ConnectedEntity;

public class ConnectedEntityLawRegisterTest
{
    private readonly ConnectedEntityLawRegisterModel _model;
    private readonly Mock<ISession> _sessionMock;
    private readonly Guid _organisationId = Guid.NewGuid();
    private readonly Guid _entityId = Guid.NewGuid();

    public ConnectedEntityLawRegisterTest()
    {
        _sessionMock = new Mock<ISession>();
        _model = new ConnectedEntityLawRegisterModel(_sessionMock.Object);
        _model.Id = _organisationId;
    }

    [Fact]
    public void OnGet_ShouldRedirectToConnectedEntitySupplierCompanyQuestion_WhenModelStateIsInvalid()
    {
        ConnectedEntityState? state = null;
        _sessionMock
            .Setup(s => s.Get<ConnectedEntityState>(Session.ConnectedPersonKey))
            .Returns(state);

        _model.ModelState.AddModelError("Error", "Model state is invalid");

        var result = _model.OnGet();

        result.Should().BeOfType<RedirectToPageResult>()
            .Which.PageName.Should().Be("ConnectedEntitySupplierCompanyQuestion");
    }

    [Fact]
    public void OnGet_ShouldReturnPageResult()
    {
        var state = DummyConnectedPersonDetails();
        
        _sessionMock
            .Setup(s => s.Get<ConnectedEntityState>(Session.ConnectedPersonKey))
            .Returns(state);

        var result = _model.OnGet();

        result.Should().BeOfType<PageResult>();
        _model.LegalForm.Should().Be("Legal-Form");
        _model.LawRegistered.Should().Be("Law-Registered");
    }

    [Fact]
    public void OnGet_ShouldRedirectToConnectedEntitySupplierCompanyQuestion_WhenSessionStateIsNull()
    {
        _sessionMock
            .Setup(s => s.Get<ConnectedEntityState>(Session.ConnectedPersonKey))
            .Returns((ConnectedEntityState?)null);

        var result = _model.OnGet();

        result.Should().BeOfType<RedirectToPageResult>()
            .Which.PageName.Should().Be("ConnectedEntitySupplierCompanyQuestion");
    }

    [Fact]
    public void OnPost_ShouldReturnPage_WhenModelStateIsInvalid()
    {
        _sessionMock
           .Setup(s => s.Get<ConnectedEntityState>(Session.ConnectedPersonKey))
           .Returns((ConnectedEntityState?)null);

        _model.ModelState.AddModelError("Error", "Model state is invalid");

        var result = _model.OnPost();

        result.Should().BeOfType<RedirectToPageResult>();
    }


    [Theory]
    [InlineData("ConnectedEntityCompanyQuestion")]
    public void OnPost_ShouldRedirectToExpectedPage(string expectedRedirectPage)
    {
        var state = DummyConnectedPersonDetails();

        _sessionMock.Setup(s => s.Get<ConnectedEntityState>(Session.ConnectedPersonKey)).
            Returns(state);

        var result = _model.OnPost();

        var redirectToPageResult = result.Should().BeOfType<RedirectToPageResult>().Subject;

        result.Should().BeOfType<RedirectToPageResult>()
            .Which.PageName.Should().Be(expectedRedirectPage);
    }

    [Fact]
    public void OnPost_ShouldUpdateSessionState_WhenModelStateIsValid()
    {
        var state = DummyConnectedPersonDetails();

        _sessionMock
            .Setup(s => s.Get<ConnectedEntityState>(Session.ConnectedPersonKey))
            .Returns(state);

        _model.OnPost();

        _sessionMock.Verify(s => s.Set(Session.ConnectedPersonKey, It.Is<ConnectedEntityState>(st => st.ConnectedEntityType == Constants.ConnectedEntityType.Organisation)), Times.Once);
    }

    private ConnectedEntityState DummyConnectedPersonDetails()
    {
        var connectedPersonDetails = new ConnectedEntityState
        {
            ConnectedEntityId = _entityId,
            SupplierHasCompanyHouseNumber = true,
            SupplierOrganisationId = _organisationId,
            ConnectedEntityType = Constants.ConnectedEntityType.Organisation,
            ConnectedEntityOrganisationCategoryType = Constants.ConnectedEntityOrganisationCategoryType.RegisteredCompany,
            OrganisationName = "Org_name",
            LegalForm = "Legal-Form",
            LawRegistered = "Law-Registered"
        };

        return connectedPersonDetails;
    }

    private static List<ConnectedEntityLookup> ConnectedEntities =>
    [
         new(Guid.NewGuid(), "e1",It.IsAny<Uri>()),
         new(Guid.NewGuid(), "e2",It.IsAny<Uri>()),
    ];

    private static SupplierInformation SupplierInformationClientModel => new(
            organisationName: "FakeOrg",
            supplierType: SupplierType.Organisation,
            operationTypes: null,
            completedRegAddress: true,
            completedPostalAddress: false,
            completedVat: false,
            completedWebsiteAddress: false,
            completedEmailAddress: true,
            completedQualification: false,
            completedTradeAssurance: false,
            completedOperationType: false,
            completedLegalForm: false,
            completedConnectedPerson: false,
            tradeAssurances: null,
            legalForm: null,
            qualifications: null);

    private static Organisation.WebApiClient.Organisation OrganisationClientModel(Guid id) =>
        new(
            additionalIdentifiers: [new Identifier(id: "FakeId", legalName: "FakeOrg", scheme: "VAT", uri: null)],
            addresses: null,
            contactPoint: new ContactPoint(email: "test@test.com", faxNumber: null, name: null, telephone: null, url: new Uri("https://xyz.com")),
            id: id,
            identifier: null,
            name: "Test Org",
            roles: [PartyRole.Supplier]);
}