using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using HtmlAgilityPack;
using Iconlook.Entity;
using ServiceStack;

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
            var rank = new Stack<int>(Enumerable.Range(1, 60));
            var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
            var data = (from t in html.DocumentNode.SelectNodes("//tbody")
                        from r in t.SelectNodes("tr")
                        select new PRep
                        {
                            Rank = rank.Pop(),
                            Created = r.SelectNodes("td")[1].InnerText.Trim(),
                            Name = r.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                            Id = r.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                            LogoUrl = $"https://icon.community{r.SelectSingleNode("td/div/img").GetAttributeValue("src", null)}",
                            Location = r.SelectNodes("td")[3].InnerText.Trim().Split(',').LastOrDefault().ToLower().ToTitleCase()
                        }).OrderBy(x => x.Rank).Shuffle().Take(12).ToList();
            return data;
        }
    }
}