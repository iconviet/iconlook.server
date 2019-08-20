using System;
using Agiper.Object;
using FluentValidation;
using Iconlook.Object;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(Prep))]
    public class Prep : EntityBase<Prep>, IHasState<PrepState>, IHasTimestamp, IHasHash
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [ForeignKey(typeof(PrepState_))]
        public PrepState State { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        public DateTimeOffset Joined { get; set; }

        public int Votes { get; set; }

        public int CScore { get; set; }

        public int Voters { get; set; }

        [StringLength(64)]
        public string Hash { get; set; }

        public int Position { get; set; }

        public bool Direction { get; set; }

        public DateTimeOffset LastSeen { get; set; }

        public int MissedBlocks { get; set; }

        public int ProducedBlocks { get; set; }

        public double UptimePercentage { get; set; }

        public double SupplyPercentage { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<Prep> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
            validator.RuleFor(x => x.State).IsInEnum().NotEqual(PrepState.Empty);
            validator.RuleFor(x => x.Hash).Length(64).When(x => x.State != PrepState.Empty);
        }
    }
}