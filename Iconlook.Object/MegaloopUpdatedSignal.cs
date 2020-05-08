using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopUpdatedSignal : SignalBase<MegaloopUpdatedSignal>
    {
        public MegaloopResponse Megaloop { get; set; }

        protected override void AddRules(Validator<MegaloopUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Megaloop).SetValidator(MegaloopResponse.Validator);
        }
    }
}