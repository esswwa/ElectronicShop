using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Data.Model
{
    public partial class Firms
    {
        public int Idfirms { get; set; }

        public string Firm { get; set; } = null!;

        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}
