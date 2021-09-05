namespace App.DbAccess.Models
{
    public class ProductView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int BuyCount { get; set; }
    }
}
