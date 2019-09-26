namespace Iconlook.Service.Web.Sources
{
    public static class Source
    {
        public static BlockSource Blocks { get; }
        public static BlockchainSource Blockchain { get; }
        public static TransactionSource Transactions { get; }

        static Source()
        {
            Blocks = new BlockSource(x => x.Height);
            Transactions = new TransactionSource(x => x.Hash);
            Blockchain = new BlockchainSource(x => x.BlockHeight);
        }
    }
}