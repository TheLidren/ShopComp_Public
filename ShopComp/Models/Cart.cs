namespace ShopComp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public Tovar Tovar { get; set; }
        public int Quantity { get; set; }
        public User Users { get; set; }
        public bool Status { get; set; }
        public bool Submitted { get; set; }
        public int OrderDetailsID { get; set; }
        public bool Condition { get; set; }

    }

}
