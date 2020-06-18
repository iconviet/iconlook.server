namespace Iconlook.Shared.Tracker
{
    public class PageResponse<T>
    {
        public T Data { get; set; }
        public int ListSize { get; set; }
        public int TotalSize { get; set; }
        public string Result { get; set; }
        public string Description { get; set; }
    }
}