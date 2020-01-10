using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class AddressResponse : ResponseBase<AddressResponse>
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }
        public decimal Voted { get; set; }
        public decimal Staked { get; set; }
        public decimal Unstaking { get; set; }
        public decimal Available { get; set; }
        public string Description { get; set; }
        public decimal IcxBalance { get; set; }
        public decimal IscoreBalance { get; set; }
        public long UnstakedBlockHeight { get; set; }
        public long RequestedBlockHeight { get; set; }

        protected override void AddRules(Validator<AddressResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
            validator.RuleFor(x => x.Hash).NotEmpty();
        }
    }
}