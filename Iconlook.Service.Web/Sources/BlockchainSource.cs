using System;
using DynamicData;
using Iconlook.Object;

namespace Iconlook.Service.Web.Sources
{
    public class BlockchainSource : SourceCache<BlockchainResponse, long>
    {
        public BlockchainSource(Func<BlockchainResponse, long> selector) : base(selector)
        {
        }
    }
}