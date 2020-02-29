using System;
using Agiper.Object;
using FluentValidation;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(PRepHistory))]
    public class PRepHistory : EntityBase<PRepHistory>, IHasId<long>, IHasTimestamp
    {
        [PrimaryKey]
        public long Id { get; set; }

        [StringLength(100)]
        [ForeignKey(typeof(PRep))]
        public string Address { get; set; }

        public long Votes { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<PRepHistory> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Address).NotEmpty().Length(42);
        }
    }
}