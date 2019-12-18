using System;
using Agiper.Object;
using FluentValidation;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(Transaction))]
    public class Transaction : EntityBase<Transaction>, IHasTimestamp
    {
        [PrimaryKey]
        [StringLength(66)]
        public string Id { get; set; }

        [Required]
        [References(typeof(Block))]
        public long Block { get; set; }

        [Required]
        [StringLength(42)]
        public string From { get; set; }

        [Required]
        [StringLength(42)]
        public string To { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<Transaction> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotNull().Length(66);
        }
    }
}