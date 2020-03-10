using System.Collections.Generic;

namespace Iconlook.Client.Tracker
{
    public class DelegateListResponse
    {
        public int ListSize { get; set; }
        public int TotalSize { get; set; }
        public List<DelegateResponse> Data { get; set; }
    }
}