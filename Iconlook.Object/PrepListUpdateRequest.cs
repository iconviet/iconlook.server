using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    // TODO: switch route to /prep
    [Route("/v1/prep/list", "PUT")]
    public class PRepListUpdateRequest : RequestBase<PRepListUpdateRequest>, IPut
    {
        public int Score { get; set; }
        public string Id { get; set; }
        public string Location { get; set; }
    }
}