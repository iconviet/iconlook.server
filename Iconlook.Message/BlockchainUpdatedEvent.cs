using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Message
{
    public class BlockchainUpdatedEvent : EventBase<BlockchainUpdatedEvent>
    {
        public BlockchainResponse Blockchain { get; set; }

        protected override void AddRules(Validator<BlockchainUpdatedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Blockchain).SetValidator(BlockchainResponse.Validator);
        }
    }
}