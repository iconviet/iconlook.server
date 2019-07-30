using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Entity;

namespace Iconlook.Service.Api
{
    public class PrepService : ServiceBase
    {
        public async Task<List<Prep>> GetLatestPrepsAsync()
        {
            return Enumerable.Range(1, 12).Select(x => new Prep
            {
                Name = new[] { "ALFKI", "ANANTR", "ANTON", "BLONP", "BOLID" }[new Random().Next(5)],
                Location = new[] { "ALFKI", "ANANTR", "ANTON", "BLONP", "BOLID" }[new Random().Next(5)]
            }).ToList();
        }
    }
}