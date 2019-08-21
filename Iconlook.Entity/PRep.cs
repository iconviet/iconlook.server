using System;
using Agiper.Object;
using FluentValidation;
using Iconlook.Object;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(Prep))]
    public class Prep : EntityBase<Prep>, IHasId<long>, IHasTimestamp, IHasState<PrepState>
    {
        [PrimaryKey]
        public long Id { get; set; }

        [StringLength(18)]
        public string IdExternal { get; set; }

        [ForeignKey(typeof(PrepState_))]
        public PrepState State { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(42)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        public DateTimeOffset Joined { get; set; }

        public int Votes { get; set; }

        public int Voters { get; set; }

        public int Score { get; set; }

        public string Goals { get; set; }

        public int Position { get; set; }

        public string Entity { get; set; }

        public string Identity { get; set; }

        public string Hosting { get; set; }
        
        public string Regions { get; set; }

        public bool Testnet { get; set; }

        public int Size { get; set; }

        public bool Direction { get; set; }

        public double Balance { get; set; }

        public DateTimeOffset LastSeen { get; set; }

        public int RejectedBlocks { get; set; }

        public int ProducedBlocks { get; set; }

        public double UptimePercentage { get; set; }

        public double SupplyPercentage { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<Prep> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
            validator.RuleFor(x => x.State).IsInEnum().NotEqual(PrepState.Empty);
            validator.RuleFor(x => x.Address).Length(4).When(x => x.State != PrepState.Empty);
        }
    }
}