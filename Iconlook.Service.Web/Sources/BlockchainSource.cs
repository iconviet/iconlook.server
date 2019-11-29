using DynamicData;
using Iconlook.Object;
using System;

namespace Iconlook.Service.Web.Sources
{
    public class BlockchainSource : SourceCache<BlockchainResponse, long>
    {
        public BlockchainSource(Func<BlockchainResponse, long> selector) : base(selector)
        {
        }
    }
}