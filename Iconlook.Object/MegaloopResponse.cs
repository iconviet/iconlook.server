using System.Collections.Generic;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopResponse : ResponseBase<MegaloopResponse>
    {
        public int PlayerCount { get; set; }
        public decimal JackpotSize { get; set; }
        public decimal JackpotSubsidy { get; set; }
        public decimal TotalJackpotSize { get; set; }
        public decimal TotalJackpotSizeUsd { get; set; }
        public MegaloopPlayerResponse LastPlayer { get; set; }
        public MegaloopWinnerResponse LastWinner { get; set; }
        public List<MegaloopPlayerResponse> Players { get; set; }
        public List<MegaloopWinnerResponse> Winners { get; set; }

        public MegaloopResponse()
        {
            Players = new List<MegaloopPlayerResponse>();
            Winners = new List<MegaloopWinnerResponse>();
        }

        protected override void AddRules(Validator<MegaloopResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleForEach(x => x.Players).SetValidator(MegaloopPlayerResponse.Validator);
            validator.RuleForEach(x => x.Winners).SetValidator(MegaloopPlayerResponse.Validator);
        }
    }
}