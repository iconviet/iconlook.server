using System.Collections.Generic;
using Agiper.Object;
using Agiper.Server;
using FluentValidation;
using Iconlook.Entity;

namespace Iconlook.Message
{
    public class BlockProducedEvent : EventBase<BlockProducedEvent>
    {
        public Block Block { get; set; }
        public List<Transaction> Transactions { get; set; }

        public BlockProducedEvent()
        {
            Transactions = new List<Transaction>();
        }

        protected override void AddRules(Validator<BlockProducedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Block).SetValidator(Block.Validator);
        }
    }

    public class BlockProducedEventValidator : AbstractValidator<BlockProducedEvent>
    {
        public BlockProducedEventValidator()
        {
            Include(BlockProducedEvent.Validator);
        }
    }
}