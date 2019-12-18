using System.Linq;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class PeerService : ServiceBase
    {
        public object Get(PeerListRequest request)
        {
            return new ListResponse<PeerResponse>(Redis.Instance().As<PeerResponse>().GetAll().Take(request.Take).OrderBy(x => x.Rank));
        }
    }
}