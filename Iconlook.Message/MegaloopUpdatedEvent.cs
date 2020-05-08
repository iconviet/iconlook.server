using Iconlook.Object;
using Iconviet.Object;
using Iconviet.Server;

namespace Iconlook.Message
{
    public class MegaloopUpdatedEvent : EventBase<MegaloopUpdatedEvent>
    {
        public MegaloopResponse Megaloop { get; set; }

        protected override void AddRules(Validator<MegaloopUpdatedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Megaloop).SetValidator(MegaloopResponse.Validator);
        }
    }
}