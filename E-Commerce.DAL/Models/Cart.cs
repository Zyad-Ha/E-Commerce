using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL.Models
{
    public class Cart
    {

        [Key]
        public int Id { get; set; }
        public int ApplicationUser_Id { get; set; }
        [ForeignKey(nameof(ApplicationUser_Id))]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<CartItem> cartItems { get; set; } = new HashSet<CartItem>();
    }
}
