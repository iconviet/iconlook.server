using System.Collections.Generic;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopResponse : ScoreResponse<MegaloopResponse>
    {
        public decimal Limit { get; set; }
        public int PlayerCount { get; set; }
        public decimal JackpotSize { get; set; }
        public List<MegaloopPlayerResponse> Players { get; set; }

        protected override void AddRules(Validator<MegaloopResponse> validator)
        {
            base.AddRules(validator);
            // validator.RuleFor(x => x.Limit).NotEmpty();
        }
    }
}