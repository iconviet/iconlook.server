using FluentValidation;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopPlayerResponse : ResponseBase<MegaloopPlayerResponse>
    {
        public decimal Chance { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }

        protected override void AddRules(Validator<MegaloopPlayerResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Amount).NotEmpty();
            validator.RuleFor(x => x.Address).NotEmpty();
        }
    }
}