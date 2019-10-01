using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class BlockProducedSignal : SignalBase<BlockProducedSignal>
    {
        public BlockResponse Block { get; set; }

        protected override void AddRules(Validator<BlockProducedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Block).NotNull().SetValidator(BlockResponse.Validator);
        }
    }
}