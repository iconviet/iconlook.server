using System.Collections.Generic;
using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class BlockUpdatedSignal : SignalBase<BlockUpdatedSignal>
    {
        public BlockResponse Block { get; set; }
        public List<TransactionResponse> Transactions { get; set; }

        public BlockUpdatedSignal()
        {
            Transactions = new List<TransactionResponse>();
        }

        protected override void AddRules(Validator<BlockUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Block).NotNull().SetValidator(BlockResponse.Validator);
        }
    }
}