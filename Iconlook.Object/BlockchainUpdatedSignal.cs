using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class BlockchainUpdatedSignal : SignalBase<BlockchainUpdatedSignal>
    {
        public BlockchainResponse Blockchain { get; set; }

        protected override void AddRules(Validator<BlockchainUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Blockchain).NotNull().SetValidator(BlockchainResponse.Validator);
        }
    }
}