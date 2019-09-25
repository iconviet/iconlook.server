using System;
using DynamicData;
using Iconlook.Object;

namespace Iconlook.Service.Web.Sources
{
    public class BlockSource : SourceCache<BlockResponse, int>
    {
        public BlockSource(Func<BlockResponse, int> selector) : base(selector)
        {
        }
    }
}