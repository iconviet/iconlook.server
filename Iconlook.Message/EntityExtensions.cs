using Iconlook.Entity;
using Iconlook.Object;
using ServiceStack;

namespace Iconlook.Message
{
    public static class EntityExtensions
    {
        public static PRepResponse ToResponse(this PRep instance)
        {
            return instance.ConvertTo<PRepResponse>();
        }

        public static TransactionResponse ToResponse(this Transaction instance)
        {
            return instance.ConvertTo<TransactionResponse>();
        }

        public static BlockResponse ToResponse(this Block instance)
        {
            return instance.ConvertTo<BlockResponse>().ThenDo(x => x.PRepName = "ICONVIET");
        }
    }
}