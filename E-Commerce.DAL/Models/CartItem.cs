using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int Product_Id { get; set; }
        [ForeignKey(nameof(Product_Id))]
        public Product Product { get; set; }

        public int Cart_Id { get; set; }
        [ForeignKey(nameof(Cart_Id))]
        public Cart Cart { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
