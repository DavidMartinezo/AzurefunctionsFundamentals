using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartList.Models
{
    internal class ShopingCartItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.Now;
        public string ItemName { get; set; }
        public bool Collected { get; set; }
        public string Category { get; set; } = "";
    }

    internal class CreateShoppingCartItem
    {
        public string ItemName { get;set; }
        public string Category { get; set; } = "";

    }

    internal class UpdateShoppingCartItem
    {
        //public string ItemName { get; set; }
        public bool Collected { get; set; }
    }
}
