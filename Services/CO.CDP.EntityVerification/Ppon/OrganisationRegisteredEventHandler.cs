using CO.CDP.EntityVerification.Events;
using CO.CDP.EntityVerification.Persistence;
using CO.CDP.MQ;
using Identifier = CO.CDP.EntityVerification.Persistence.Identifier;

namespace CO.CDP.EntityVerification.Ppon;

public class OrganisationRegisteredEventHandler(IPponService pponService,
    IPponRepository pponRepository,
    IPublisher publisher)
    : IEventHandler<OrganisationRegistered>
{
    public async Task Handle(OrganisationRegistered @event)
    {
        Persistence.Ppon newPpon = new() {
            IdentifierId = pponService.GeneratePponId(),
            Name = @event.Name,
            OrganisationId = @event.Id
        };

        newPpon.Identifiers = Identifier.GetPersistenceIdentifiers(@event.AllIdentifiers(), newPpon);

        PponGenerated pponGenerated = new()
        {
            Id = newPpon.IdentifierId,
            LegalName = @event.Identifier.LegalName,
            Scheme = @event.Identifier.Scheme
        };

        pponRepository.Save(newPpon);
        await publisher.Publish(pponGenerated);
    }
}