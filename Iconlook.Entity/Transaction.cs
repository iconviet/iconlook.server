using System;
using Agiper.Object;
using FluentValidation;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(Transaction))]
    public class Transaction : EntityBase<Transaction>, IHasId<long>, IHasTimestamp
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string To { get; set; }

        public long Block { get; set; }

        public string From { get; set; }

        public decimal Fee { get; set; }

        public string Hash { get; set; }

        public decimal Amount { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<Transaction> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
        }
    }
}