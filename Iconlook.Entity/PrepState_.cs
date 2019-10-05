using Iconlook.Object;
using ServiceStack.DataAnnotations;

namespace Iconlook.Entity
{
    [Alias(nameof(PRepState))]
    public class PRepState_ : EntityBase<PRepState_>
    {
        [PrimaryKey]
        public PRepState State { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}