using System.Collections.Generic;
using Agiper.Object;

namespace Iconlook.Object
{
    public class PeersUpdatedSignal : SignalBase<PeersUpdatedSignal>
    {
        public List<PeerResponse> Peers { get; set; }

        protected override void AddRules(Validator<PeersUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleForEach(x => x.Peers).SetValidator(PeerResponse.Validator);
        }
    }
}