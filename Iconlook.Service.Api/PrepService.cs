using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Serilog;
using ServiceStack.OrmLite;

namespace Iconlook.Service.Api
{
    public class PRepService : ServiceBase
    {
        public void Put(PRepListUpdateRequest request)
        {
            Log.Information("Update", request);
        }

        public async Task<object> Get(PRepListRequest request)
        {
            using (var db = Db.Instance())
            using (var redis = Redis.Instance())
            {
                if (request.Filter.HasValue())
                {
                    var name = request.Filter
                        .Replace("substringof('", string.Empty)
                        .Replace("',tolower(Name))", string.Empty);
                    var preps = redis.As<PRepResponse>().GetAll()
                        .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                        .Skip(request.Skip).Take(request.Take);
                    return new ListResponse<PRepResponse>(preps)
                    {
                        Skip = request.Skip,
                        Take = request.Take,
                        Count = preps.Count()
                    };
                }
                if (request.Edit.HasValue() && request.Edit != "all")
                {
                    var preps = await db.SelectAsync(
                        db.From<PRep>().Where(x => x.Id == request.Edit));
                    return new ListResponse<PRepResponse>(preps.ConvertAll(x => x.ToResponse()))
                    {
                        Skip = 0,
                        Take = 1,
                        Count = 1
                    };
                }
                {
                    var preps = redis.As<PRepResponse>().GetAll().OrderBy(x => x.Ranking).ToList();
                    return new ListResponse<PRepResponse>(preps.Skip(request.Skip).Take(request.Take))
                    {
                        Skip = request.Skip,
                        Take = request.Take,
                        Count = preps.Count
                    };
                }
            }
        }
    }
}