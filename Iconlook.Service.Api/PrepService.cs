using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using HtmlAgilityPack;
using Iconlook.Entity;
using Iconlook.Message;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PRepService : ServiceBase
    {
        public async Task<List<PRepResponse>> Any(PRepListRequest request)
        {
            var data = new List<PRepResponse>();
            try
            {
                var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
                var query = from t in html.DocumentNode.SelectNodes("//tbody")
                            from r in t.SelectNodes("tr")
                            select r;
                var rank = new Stack<int>(Enumerable.Range(1, query.Count()));
                data = query.Select(x => new PRepResponse
                {
                    Position = rank.Pop(),
                    Created = x.SelectNodes("td")[1].InnerText.Trim(),
                    Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                    Id = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                    Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').LastOrDefault().ToLower().ToTitleCase(),
                    LogoUrl = $"images/preps/{x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3)}.png"
                }).OrderBy(x => x.Position).Reverse().Take(request.Take == 0 ? 22 : 0).ToList();
            }
            catch (Exception)
            {
            }
            return data;
        }

        public async Task<List<PRep>> GetLatestPRepsAsync(int take = 22)
        {
            var data = new List<PRep>();
            try
            {
                var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
                var query = from t in html.DocumentNode.SelectNodes("//tbody")
                            from r in t.SelectNodes("tr")
                            select r;
                var position = new Stack<int>(Enumerable.Range(1, query.Count()));
                data = query.Select(x => new PRep
                {
                    Position = position.Pop(),
                    CScore = new Random().Next(0, 128),
                    Voters = new Random().Next(100, 1000),
                    Votes = new Random().Next(1000000, 10000000),
                    Direction = new Random().NextDouble() >= 0.5,
                    UptimePercentage = new Random().NextDouble(),
                    LastSeen = $"{new Random().Next(1, 60)}s ago",
                    Created = x.SelectNodes("td")[1].InnerText.Trim(),
                    Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                    Id = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                    Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').LastOrDefault().ToLower().ToTitleCase(),
                    LogoUrl = $"images/preps/{x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3)}.png"
                }.ThenDo(o => o.SupplyPercentage = (double) o.Votes / 490000000)).Distinct().OrderBy(x => x.Position).Reverse().Take(take).ToList();
            }
            catch (Exception)
            {
            }
            return data;
        }
    }
}