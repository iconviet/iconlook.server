using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class DelegateResponse : ResponseBase<DelegateResponse>
    {
        public string Address { get; set; }
        public decimal Amount { get; set; }

        protected override void AddRules(Validator<DelegateResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Address).NotEmpty();
        }
    }
}