namespace Iconlook.Service.Web.Sources
{
    public static class Source
    {
        public static BlockSource Blocks { get; }
        public static ChainSource Chain { get; }
        public static TransactionSource Transactions { get; }

        static Source()
        {
            Blocks = new BlockSource(x => x.Height);
            Transactions = new TransactionSource(x => x.Hash);
            Chain = new ChainSource(x => x.BlockHeight);
        }
    }
}