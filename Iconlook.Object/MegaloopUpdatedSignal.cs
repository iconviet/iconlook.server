using FluentValidation;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopUpdatedSignal : SignalBase<MegaloopUpdatedSignal>
    {
        public int PlayerCount { get; set; }
        public decimal JackpotSize { get; set; }
        public decimal JackpotSizeUsd { get; set; }
        public MegaloopPlayerResponse LastPlayer { get; set; }
        public MegaloopPlayerResponse LastWinner { get; set; }

        protected override void AddRules(Validator<MegaloopUpdatedSignal> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.LastPlayer).NotEmpty();
            validator.RuleFor(x => x.LastWinner).NotEmpty();
            validator.RuleFor(x => x.JackpotSize).NotEmpty();
            validator.RuleFor(x => x.PlayerCount).NotEmpty();
            validator.RuleFor(x => x.JackpotSizeUsd).NotEmpty();
        }
    }
}