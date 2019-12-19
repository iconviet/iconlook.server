using System;
using Agiper.Object;
using FluentValidation;
using Iconlook.Object;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(PRep))]
    public class PRep : EntityBase<PRep>, IHasTimestamp, IHasState<PRepState>
    {
        [PrimaryKey]
        [StringLength(100)]
        public string Id { get; set; }

        public long Ranking { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string LogoUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string P2PEndpoint { get; set; }

        [ForeignKey(typeof(PRepState_))]
        public PRepState State { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        public DateTimeOffset Joined { get; set; }

        public long Votes { get; set; }

        public long Voters { get; set; }

        public long Score { get; set; }

        public string Goals { get; set; }

        public string Entity { get; set; }

        public string Identity { get; set; }

        public string Hosting { get; set; }

        public string Regions { get; set; }

        public bool Testnet { get; set; }

        public long Size { get; set; }

        public bool Direction { get; set; }

        public double Balance { get; set; }

        public DateTimeOffset LastSeen { get; set; }

        public long MissedBlocks { get; set; }

        public long ProducedBlocks { get; set; }

        public double ProductivityPercentage { get; set; }

        public double DelegatedPercentage { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<PRep> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty().Length(42);
            validator.RuleFor(x => x.State).IsInEnum().NotEqual(PRepState.Empty);
        }
    }
}