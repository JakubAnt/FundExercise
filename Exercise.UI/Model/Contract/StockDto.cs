namespace Exercise.UI.Model.Contract
{
    public class StockDto
    {
        public StockType StockType { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal MarektValue { get; set; }

        public decimal TransactionCost { get; set; }
    }
}