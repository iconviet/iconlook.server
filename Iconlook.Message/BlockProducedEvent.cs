using System.Collections.Generic;
using Agiper.Object;
using Agiper.Server;
using FluentValidation;
using Iconlook.Object;

namespace Iconlook.Message
{
    public class BlockProducedEvent : EventBase<BlockProducedEvent>
    {
        public BlockResponse Block { get; set; }
        public List<TransactionResponse> Transactions { get; set; }

        public BlockProducedEvent()
        {
            Transactions = new List<TransactionResponse>();
        }

        protected override void AddRules(Validator<BlockProducedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Block).SetValidator(BlockResponse.Validator);
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