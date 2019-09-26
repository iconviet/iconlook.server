using System;
using DynamicData;
using Iconlook.Object;

namespace Iconlook.Service.Web.Sources
{
    public class TransactionSource : SourceCache<TransactionResponse, string>
    {
        public TransactionSource(Func<TransactionResponse, string> selector) : base(selector)
        {
        }
    }
}