using CO.CDP.Organisation.WebApi.Model;
using CO.CDP.Organisation.WebApi.UseCase;
using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CO.CDP.Organisation.WebApi.Api;
internal record OrganisationDetails
{
    [Required] public required OrganisationIdentifier Identifier { get; init; }
    [Required] public required List<OrganisationIdentifier> AdditionalIdentifiers { get; init; }
    [Required(AllowEmptyStrings = true)] public required string Name { get; init; }
    [Required] public required OrganisationAddress Address { get; init; }
    [Required] public required OrganisationContactPoint ContactPoint { get; init; }
    [Required] public required List<Role> Roles { get; init; }
}

internal record NewOrganisation
{
    [Required] public required OrganisationIdentifier Identifier { get; init; }
    [Required] public required List<OrganisationIdentifier> AdditionalIdentifiers { get; init; }
    [Required(AllowEmptyStrings = true)] public required string Name { get; init; }
    [Required] public required OrganisationAddress Address { get; init; }
    [Required] public required OrganisationContactPoint ContactPoint { get; init; }
    [Required] public required List<Role> Roles { get; init; }
}

internal enum Role
{
    ProcuringEntity,
    Supplier,
    Tenderer,
    Funder,
    Enquirer
}

public static class EndpointExtensions
{
    private static Dictionary<Guid, Model.Organisation> _organisations = Enumerable.Range(1, 5)
        .Select(_ => Guid.NewGuid())
        .ToDictionary(id => id, id => new Model.Organisation
        {
            Id = id,
            Identifier = new OrganisationIdentifier
            {
                Scheme = "CH",
                Id = $"123945123{id}",
                LegalName = "TheOrganisation",
                Uri = "https://example.com"
            },
            Name = $"Tables Limited {id}",
            AdditionalIdentifiers = [],
            Address = new OrganisationAddress
            {
                StreetAddress = $"Green Lane {id}",
                Locality = "London",
                Region = "Kent",
                CountryName = "United Kingdom",
                PostalCode = "BR8 7AA"
            },
            ContactPoint = new OrganisationContactPoint
            {
                Name = "Bobby Tables",
                Email = $"bobby+{id}@example.com",
                Telephone = "07925123123",
                FaxNumber = "07925123999",
                Url = "https://example.com"
            },
            Roles = [1],
        });

    public static void UseOrganisationEndpoints(this WebApplication app)
    {
        app.MapGet("/organisations", () => _organisations.Values.ToArray())
            .Produces<List<Model.Organisation>>(200, "application/json")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "ListOrganisations";
                operation.Description = "A list of organisations.";
                operation.Summary = "A list of organisations.";
                operation.Responses["200"].Description = "A list of organisations.";
                return operation;
            });

        app.MapPost("/organisations", async (RegisterOrganisation command, IUseCase<RegisterOrganisation, Model.Organisation> useCase) =>
            await useCase.Execute(command)
                .AndThen(organisation =>
                    organisation != null
                        ? Results.Created(new Uri($"/organisations/{organisation.Id}", UriKind.Relative), organisation)
                        : Results.Problem("Organisation could not be created due to an internal error"))
        )
        .Produces<Model.Organisation>(201, "application/json")
        .ProducesProblem(500)
        .WithOpenApi(operation =>
        {
            operation.OperationId = "CreateOrganisation";
            operation.Description = "Create a new organisation.";
            operation.Summary = "Create a new organisation.";
            operation.Responses["201"].Description = "Organisation created successfully.";
            return operation;
        });

        app.MapGet("/organisations/{organisationId}", (Guid organisationId) =>
            {
                try
                {
                    return Results.Ok(_organisations[organisationId]);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound();
                }
            })
            .Produces<Model.Organisation>(200, "application/json")
            .Produces(404)
            .WithOpenApi(operation =>
            {
                operation.OperationId = "GetOrganisation";
                operation.Description = "Get a organisation by ID.";
                operation.Summary = "Get a organisation by ID.";
                operation.Responses["200"].Description = "Organisation details.";
                return operation;
            });
        app.MapPut("/organisations/{organisationId}",
                (Guid organisationId, UpdateOrganisation updatedOrganisation) =>
                {
                    _organisations[organisationId] = new Model.Organisation
                    {
                        Id = organisationId,
                        Identifier = updatedOrganisation.Identifier,
                        Name = updatedOrganisation.Name,
                        AdditionalIdentifiers = updatedOrganisation.AdditionalIdentifiers,
                        Address = updatedOrganisation.Address,
                        ContactPoint = updatedOrganisation.ContactPoint,
                        Roles = updatedOrganisation.Roles,
                    };
                    return Results.Ok(_organisations[organisationId]);
                })
            .Produces<Model.Organisation>(200, "application/json")
            .WithOpenApi(operation =>
            {
                operation.OperationId = "UpdateOrganisation";
                operation.Description = "Update a organisation";
                operation.Summary = "Update a organisation";
                operation.Responses["200"].Description = "Organisation updated.";
                return operation;
            });
        app.MapDelete("/organisations/{organisationId}", (Guid organisationId) =>
            {
                _organisations.Remove(organisationId);
                return Results.NoContent();
            })
            .Produces(204)
            .WithOpenApi(operation =>
            {
                operation.OperationId = "DeleteOrganisation";
                operation.Description = "Delete a organisation.";
                operation.Summary = "Delete a organisation.";
                operation.Responses["204"].Description = "Organisation deleted.";
                return operation;
            });
    }
}

public static class ApiExtensions
{
    public static void DocumentOrganisationApi(this SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "1.0.0.0",
            Title = "Organisation Management API",
            Description = "API for creating, updating, deleting, and listing organisations, including a lookup feature against organisation identifiers.",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Example Contact",
                Url = new Uri("https://example.com/contact")
            },
            License = new OpenApiLicense
            {
                Name = "Example License",
                Url = new Uri("https://example.com/license")
            }
        });
        options.CustomSchemaIds(type =>
        {
            var typeMap = new Dictionary<Type, string>
                {
                    { typeof(RegisterOrganisation), "NewOrganisation" },
                    { typeof(UpdateOrganisation), "UpdatedOrganisation" },
                };
            return typeMap.GetValueOrDefault(type, type.Name);
        });
    }

}