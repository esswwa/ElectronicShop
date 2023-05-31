using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Data.Model
{
    public partial class Imageproduct
    {
        public int IdimageProduct { get; set; }

        public int IdProduct { get; set; }

        public string Image { get; set; } = null!;

    }
}
