using Agiper.Object;

namespace Iconlook.Object
{
    public class PeersUpdatedSignal : SignalBase<PeersUpdatedSignal>
    {
        public PeerResponse Busy { get; set; }

        protected override void AddRules(Validator<PeersUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Busy).SetValidator(PeerResponse.Validator);
        }
    }
}