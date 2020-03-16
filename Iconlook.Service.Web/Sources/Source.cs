namespace Iconlook.Service.Web.Sources
{
    public static class Source
    {
        public static BlockSource Blocks { get; }
        public static ChainSource Chains { get; }
        public static TransactionSource Transactions { get; }

        static Source()
        {
            Blocks = new BlockSource(x => x.Height);
            Chains = new ChainSource(x => x.BlockHeight);
            Transactions = new TransactionSource(x => x.Hash);
        }
    }
}