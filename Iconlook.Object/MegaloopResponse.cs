using System.Collections.Generic;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopResponse : ScoreResponse<MegaloopResponse>
    {
        public int PlayerCount { get; set; }
        public decimal JackpotSize { get; set; }
        public decimal JackpotSizeUsd { get; set; }
        public MegaloopPlayerResponse LastPlayer { get; set; }
        public MegaloopWinnerResponse LastWinner { get; set; }
        public List<MegaloopPlayerResponse> Players { get; set; }

        protected override void AddRules(Validator<MegaloopResponse> validator)
        {
            base.AddRules(validator);
            // validator.RuleFor(x => x.Limit).NotEmpty();
        }
    }
}