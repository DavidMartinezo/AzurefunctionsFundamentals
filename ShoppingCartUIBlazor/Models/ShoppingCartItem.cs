namespace ShoppingCartUIBlazor.Models
{
    public class ShoppingCartItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; }
        public string ItemName { get; set; }
        public bool Collected { get; set; }
        public string Category { get; set; }
    }

    
}
