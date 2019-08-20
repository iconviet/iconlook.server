using Iconlook.Object;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(PrepState))]
    public class PrepState_ : EntityBase<PrepState_>
    {
        [PrimaryKey]
        public PrepState State { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}