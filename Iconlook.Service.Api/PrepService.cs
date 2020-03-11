using System.Linq;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;
using Serilog;

namespace Iconlook.Service.Api
{
    public class PRepService : ServiceBase
    {
        public void Put(PRepListUpdateRequest request)
        {
            Log.Information("Update", request);
        }

        public object Get(PRepListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var preps = redis.As<PRepResponse>().GetAll().AsQueryable();
                if (request.Edit.HasValue() && request.Edit != "all")
                {
                    return new ListResponse<PRepResponse>(preps.Where(x => x.Id == request.Edit))
                    {
                        Skip = 0,
                        Take = 1,
                        Count = 1
                    };
                }
                if (request.Filter.HasValue())
                {
                    switch (request.Filter)
                    {
                        case "main_prep":
                            preps = preps.Where(x => x.Ranking <= 22);
                            break;
                        case "candidate":
                            preps = preps.Where(x => x.Ranking > 100);
                            break;
                        case "controversal":
                            preps = preps.Where(x => x.Name == "controversal");
                            break;
                        case "disqualified":
                            preps = preps.Where(x => x.Name == "disqualified");
                            break;
                        case "sub_prep":
                            preps = preps.Where(x => x.Ranking > 22 && x.Ranking <= 100);
                            break;
                    }
                }
                return new ListResponse<PRepResponse>(preps
                    .OrderBy(x => x.Ranking)
                    .Skip(preps.Count() > request.Take ? request.Skip : 0).Take(request.Take))
                {
                    Take = request.Take,
                    Count = preps.Count(),
                    Skip = preps.Count() > request.Take ? request.Skip : 0
                };
            }
        }
    }
}