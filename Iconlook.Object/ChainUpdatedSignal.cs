using Iconviet.Object;

namespace Iconlook.Object
{
    public class ChainUpdatedSignal : SignalBase<ChainUpdatedSignal>
    {
        public ChainResponse Chain { get; set; }

        protected override void AddRules(Validator<ChainUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Chain).SetValidator(ChainResponse.Validator);
        }
    }
}