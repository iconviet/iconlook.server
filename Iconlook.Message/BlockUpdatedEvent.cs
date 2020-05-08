using System.Collections.Generic;
using FluentValidation;
using Iconlook.Object;
using Iconviet.Object;
using Iconviet.Server;

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
            validator.RuleForEach(x => x.Transactions).SetValidator(TransactionResponse.Validator);
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