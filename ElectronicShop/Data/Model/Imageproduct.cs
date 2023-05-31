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

        public string IdProduct { get; set; } = null;

        public string Image { get; set; } = null!;

    }
}
