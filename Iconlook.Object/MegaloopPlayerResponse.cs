using FluentValidation;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class MegaloopPlayerResponse : ResponseBase<MegaloopPlayerResponse>
    {
        public long Block { get; set; }
        public string Address { get; set; }
        public decimal Chance { get; set; }
        public decimal Deposit { get; set; }

        protected override void AddRules(Validator<MegaloopPlayerResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Deposit).NotEmpty();
            validator.RuleFor(x => x.Address).NotEmpty();
        }
    }
}