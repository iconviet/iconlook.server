namespace Iconlook.Service.Web.Sources
{
    public static class Source
    {
        public static BlockchainSource Blockchain { get; }

        static Source()
        {
            Blockchain = new BlockchainSource(x => x.BlockHeight);
        }
    }
}