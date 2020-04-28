using System.Collections.Generic;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class PeersUpdatedSignal : SignalBase<PeersUpdatedSignal>
    {
        public List<PeerResponse> Idle { get; set; }
        public List<PeerResponse> Busy { get; set; }
        public List<PeerResponse> Sync { get; set; }
        public List<PeerResponse> Down { get; set; }

        protected override void AddRules(Validator<PeersUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleForEach(x => x.Idle).SetValidator(PeerResponse.Validator);
            validator.RuleForEach(x => x.Busy).SetValidator(PeerResponse.Validator);
            validator.RuleForEach(x => x.Sync).SetValidator(PeerResponse.Validator);
            validator.RuleForEach(x => x.Down).SetValidator(PeerResponse.Validator);
        }
    }
}