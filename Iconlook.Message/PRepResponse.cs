using Agiper.Object;

namespace Iconlook.Message
{
    public class PRepResponse : ResponseBase<PRepResponse>
    {
        public int Rank { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Created { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
    }
}