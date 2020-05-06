using Iconviet.Object;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Iconlook.Object
{
    public class MegaloopResponse : ScoreResponse<MegaloopResponse>
    {
        public decimal Limit { get; set; }
        public List<string> Players { get; set;  }

        protected override void AddRules(Validator<MegaloopResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Limit).NotEmpty();

        }
    }
}
