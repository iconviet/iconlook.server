using System;
using DynamicData;
using Iconlook.Object;

namespace Iconlook.Service.Web.Sources
{
    public class ChainSource : SourceCache<ChainResponse, long>
    {
        public ChainSource(Func<ChainResponse, long> selector) : base(selector)
        {
        }
    }
}