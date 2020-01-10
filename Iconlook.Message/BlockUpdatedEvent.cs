using System.Collections.Generic;
using Agiper.Object;
using Agiper.Server;
using FluentValidation;
using Iconlook.Object;

namespace Iconlook.Message
{
    public class BlockUpdatedEvent : EventBase<BlockUpdatedEvent>
    {
        public BlockResponse Block { get; set; }
        public List<TransactionResponse> Transactions { get; set; }

        public BlockUpdatedEvent()
        {
            Transactions = new List<TransactionResponse>();
        }

        protected override void AddRules(Validator<BlockUpdatedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Block).SetValidator(BlockResponse.Validator);
            validator.RuleFor(x => x.Transactions).ForEach(x => x.SetValidator(TransactionResponse.Validator));
        }
    }

    public class BlockUpdatedEventValidator : AbstractValidator<BlockUpdatedEvent>
    {
        public BlockUpdatedEventValidator()
        {
            Include(BlockUpdatedEvent.Validator);
        }
    }
}