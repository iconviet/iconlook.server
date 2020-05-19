using FluentValidation;
using Iconviet.Object;

namespace Iconlook.Object
{
    public abstract class ScoreResponse<T> : ResponseBase<T> where T : ScoreResponse<T>
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Description { get; set; }
        public decimal IcxBalance { get; set; }

        protected override void AddRules(Validator<T> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.Id).NotEmpty();
            validator.RuleFor(x => x.Hash).NotEmpty();
        }
    }
}