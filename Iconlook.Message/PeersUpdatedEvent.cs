using System.Collections.Generic;
using Iconlook.Object;
using Iconviet.Object;
using Iconviet.Server;

namespace Iconlook.Message
{
    public class PeersUpdatedEvent : EventBase<PeersUpdatedEvent>
    {
        public List<PeerResponse> Busy { get; set; }

        protected override void AddRules(Validator<PeersUpdatedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleForEach(x => x.Busy).SetValidator(PeerResponse.Validator);
        }
    }
}