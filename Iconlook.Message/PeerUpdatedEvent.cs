using Iconviet.Object;
using Iconviet.Server;
using Iconlook.Object;

namespace Iconlook.Message
{
    public class PeerUpdatedEvent : EventBase<PeerUpdatedEvent>
    {
        public PeerResponse Peer { get; set; }

        protected override void AddRules(Validator<PeerUpdatedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Peer).SetValidator(PeerResponse.Validator);
        }
    }
}