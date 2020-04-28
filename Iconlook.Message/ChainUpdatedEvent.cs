using Iconviet.Object;
using Iconviet.Server;
using Iconlook.Object;

namespace Iconlook.Message
{
    public class ChainUpdatedEvent : EventBase<ChainUpdatedEvent>
    {
        public ChainResponse Chain { get; set; }

        protected override void AddRules(Validator<ChainUpdatedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Chain).SetValidator(ChainResponse.Validator);
        }
    }
}