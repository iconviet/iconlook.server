using DynamicData;
using Iconlook.Object;
using System;

namespace Iconlook.Service.Web.Sources
{
    public class BlockSource : SourceCache<BlockResponse, long>
    {
        public BlockSource(Func<BlockResponse, long> selector) : base(selector)
        {
        }
    }
}