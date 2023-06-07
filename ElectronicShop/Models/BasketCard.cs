using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicShop.Models
{
    public class BasketCard
    {
        public static int IdProduct { get; set; }

        public static string NameProduct { get; set; } = null!;

        public static string ImgProduct { get; set; } = null!;

        public static int CostProduct { get; set; }

        public static int CountProduct { get; set; }

        public static int Status { get; set; }

    }
}
