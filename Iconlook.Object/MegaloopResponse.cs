using System.Collections.Generic;
using FluentValidation;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopResponse : ScoreResponse<MegaloopResponse>
    {
        public decimal Limit { get; set; }
        public List<string> Players { get; set; }

        protected override void AddRules(Validator<MegaloopResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Limit).NotEmpty();
        }
    }
}