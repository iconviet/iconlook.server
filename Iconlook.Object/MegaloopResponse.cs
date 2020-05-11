using System.Collections.Generic;
using FluentValidation;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopResponse : ResponseBase<MegaloopResponse>
    {
        public int PlayerCount { get; set; }
        public decimal JackpotSize { get; set; }
        public decimal JackpotSizeUsd { get; set; }
        public MegaloopPlayerResponse LastPlayer { get; set; }
        public MegaloopWinnerResponse LastWinner { get; set; }
        public List<MegaloopPlayerResponse> Players { get; set; }
        public List<MegaloopWinnerResponse> Winners { get; set; }

        public MegaloopResponse()
        {
            Players = new List<MegaloopPlayerResponse>();
        }

        protected override void AddRules(Validator<MegaloopResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.LastPlayer).NotEmpty();
            validator.RuleFor(x => x.LastWinner).NotEmpty();
            validator.RuleFor(x => x.JackpotSize).NotEmpty();
            validator.RuleFor(x => x.PlayerCount).NotEmpty();
            validator.RuleFor(x => x.JackpotSizeUsd).NotEmpty();
            validator.RuleForEach(x => x.Players).SetValidator(MegaloopPlayerResponse.Validator);
            validator.RuleForEach(x => x.Winners).SetValidator(MegaloopPlayerResponse.Validator);
        }
    }
}