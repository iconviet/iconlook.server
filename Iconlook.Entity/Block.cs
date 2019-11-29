using System;
using Agiper.Object;
using FluentValidation;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(Block))]
    public class Block : EntityBase<Block>, IHasTimestamp
    {
        [PrimaryKey]
        public long Height { get; set; }

        [StringLength(42)]
        public string PeerId { get; set; }

        [StringLength(66)]
        public string Hash { get; set; }

        [StringLength(66)]
        public string PrevHash { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required]
        public int Transactions { get; set; }

        [Required]
        public int ByteSize { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<Block> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Height).NotEmpty();
        }
    }
}