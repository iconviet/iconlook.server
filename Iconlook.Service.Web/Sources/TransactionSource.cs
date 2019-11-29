using DynamicData;
using Iconlook.Object;
using System;

namespace Iconlook.Service.Web.Sources
{
    public class TransactionSource : SourceCache<TransactionResponse, string>
    {
        public TransactionSource(Func<TransactionResponse, string> selector) : base(selector)
        {
        }
    }
}