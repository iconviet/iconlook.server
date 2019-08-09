using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Object;
using Agiper.Server;
using HtmlAgilityPack;
using Iconlook.Message;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PrepService : ServiceBase
    {
        public async Task<object> Any(PrepListRequest request)
        {
            var data = new ListResponse<PrepResponse>();
            try
            {
                var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
                var query = from t in html.DocumentNode.SelectNodes("//tbody")
                            from r in t.SelectNodes("tr")
                            select r;
                var position = new Stack<int>(Enumerable.Range(1, query.Count()));
                data = new ListResponse<PrepResponse>(query.Select(x => new PrepResponse
                {
                    Position = position.Pop(),
                    Score = new Random().Next(-100, 100),
                    Voters = new Random().Next(100, 1000),
                    Votes = new Random().Next(1000000, 10000000),
                    Direction = new Random().NextDouble() >= 0.5,
                    UptimePercentage = new Random().NextDouble(),
                    LastSeen = $"{new Random().Next(1, 60)}s ago",
                    Created = x.SelectNodes("td")[1].InnerText.Trim(),
                    Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                    Id = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                    Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').LastOrDefault().ToLower().ToTitleCase()
                }.ThenDo(o => o.SupplyPercentage = (double) o.Votes / 490000000)).Distinct().OrderBy(x => x.Position).Reverse().Take(request.Take).ToList());
            }
            catch (Exception)
            {
            }
            return data;
        }
    }
}