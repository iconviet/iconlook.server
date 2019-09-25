namespace Iconlook.Service.Web.Sources
{
    public static class Source
    {
        public static BlockSource Blocks { get; }

        static Source()
        {
            Blocks = new BlockSource(x => x.Height);
        }
    }
}