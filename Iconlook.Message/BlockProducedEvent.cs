using System;
using System.Collections.Generic;
using Agiper.Object;
using Agiper.Server;
using FluentValidation;
using Iconlook.Entity;

namespace Iconlook.Message
{
    public class BlockProducedEvent : EventBase<BlockProducedEvent>
    {
        public int Size { get; set; }
        public long Height { get; set; }
        public string Hash { get; set; }
        public decimal Fee { get; set; }
        public decimal Amount { get; set; }
        public string Producer { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }

        public BlockProducedEvent()
        {
            Transactions = new List<Transaction>();
        }

        protected override void AddRules(Validator<BlockProducedEvent> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Height).GreaterThan(0);
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