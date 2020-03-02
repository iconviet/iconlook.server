using System;
using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class PRepResponse : ResponseBase<PRepResponse>
    {
        public string Id { get; set; }
        public long Votes { get; set; }
        public long Score { get; set; }
        public long Voters { get; set; }
        public string Name { get; set; }
        public long Ranking { get; set; }
        public bool Testnet { get; set; }
        public string Goals { get; set; }
        public string Entity { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public bool Direction { get; set; }
        public double Balance { get; set; }
        public string Hosting { get; set; }
        public string Regions { get; set; }
        public string Location { get; set; }
        public string Identity { get; set; }
        public long MissedBlocks { get; set; }
        public string P2PEndpoint { get; set; }
        public long ProducedBlocks { get; set; }
        public long Votes24HChange { get; set; }
        public DateTimeOffset Joined { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double DelegatedPercentage { get; set; }
        public double ProductivityPercentage { get; set; }

        protected override void AddRules(Validator<PRepResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
        }
    }
}