using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Entity;

namespace Iconlook.Service.Api
{
    public class PRepService : ServiceBase
    {
        public async Task<PRep> GetPRepAsync(string address)
        {
            return new PRep { Name = "ICONVIET", Location = "Vietnam" };
        }

        public async Task<List<PRep>> GetLatestPRepsAsync()
        {
            return Enumerable.Range(1, 12).Select(x => new PRep
            {
                Name = new[] { "ALFKI", "ANANTR", "ANTON", "BLONP", "BOLID" }[new Random().Next(5)],
                Location = new[] { "ALFKI", "ANANTR", "ANTON", "BLONP", "BOLID" }[new Random().Next(5)]
            }).ToList();
        }
    }
}