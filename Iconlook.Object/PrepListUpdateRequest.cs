using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    // TODO: switch route to /prep
    [Route("/v1.0/preps", "PUT")]
    public class PrepListUpdateRequest : RequestBase<PrepListUpdateRequest>, IPut
    {
        public int Score { get; set; }
        public string Id { get; set; }
        public string Location { get; set; }
    }
}