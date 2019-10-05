using System;
using Agiper.Object;
using FluentValidation;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(Block))]
    public class Block : EntityBase<Block>, IHasId<long>, IHasTimestamp
    {
        [PrimaryKey]
        public long Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<Block> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
        }
    }
}