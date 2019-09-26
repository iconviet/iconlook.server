using System;
using DynamicData;
using Iconlook.Object;

namespace Iconlook.Service.Web.Sources
{
    public class BlockSource : SourceCache<BlockResponse, long>
    {
        public BlockSource(Func<BlockResponse, long> selector) : base(selector)
        {
        }
    }
}