using System;
using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class BlockResponse : ResponseBase<BlockResponse>
    {
        public int Size { get; set; }
        public long Height { get; set; }
        public string Hash { get; set; }
        public decimal Fee { get; set; }
        public string PeerId { get; set; }
        public string PrevHash { get; set; }
        public string PRepName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public static BlockResponse ModelType = new BlockResponse();

        protected override void AddRules(Validator<BlockResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Height).GreaterThan(0);
        }
    }
}