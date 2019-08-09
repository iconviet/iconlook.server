using Agiper.Object;

namespace Iconlook.Message
{
    public class PrepResponse : ResponseBase<PrepResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string LogoUrl { get; set; }
        public string Created { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
    }
}