using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Entity;

namespace Iconlook.Service.Api
{
    public class TransactionService : ServiceBase
    {
        public async Task<List<Transaction>> GetLatestTransactionsAsync()
        {
            return Enumerable.Range(1, 12).Select(x => new Transaction
            {
                To = new[] { "hx522b...2e84", "hx522b...2e84", "hx522b...2e84", "hx522b...2e84", "hx522b...2e84" }[new Random().Next(5)],
                From = new[] { "hx522b...2e84", "hx522b...2e84", "hx522b...2e84", "hx522b...2e84", "hx522b...2e84" }[new Random().Next(5)],
                Hash = new[] { "0x07c3....bc543", "0x07c3....bc543", "0x07c3....bc543", "0x07c3....bc543", "0x07c3....bc543" }[new Random().Next(5)],
                Amount = new decimal[] { 100, 200, 300, 400, 500 }[new Random().Next(5)],
                Fee = new decimal[] { 1, 2, 3, 4, 5 }[new Random().Next(5)]
            }).ToList();
        }
    }
}