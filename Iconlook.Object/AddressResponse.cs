using FluentValidation;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class AddressResponse : ResponseBase<AddressResponse>
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }
        public decimal Voted { get; set; }
        public string Comment { get; set; }
        public decimal Staked { get; set; }
        public AddressType Type { get; set; }
        public decimal Delegated { get; set; }
        public decimal Unstaking { get; set; }
        public decimal Available { get; set; }
        public string Description { get; set; }
        public decimal IcxBalance { get; set; }
        public decimal IscoreBalance { get; set; }

        protected override void AddRules(Validator<AddressResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
            validator.RuleFor(x => x.Hash).NotEmpty();
            validator.RuleFor(x => x.Type).IsInEnum().NotEqual(AddressType.Empty);
        }
    }
}