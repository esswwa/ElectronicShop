using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Models
{
    public class Basket
    {
        List<Product> products { get; set; }
        List<int> count { get; set; }
        List<double> cost { get; set; }
    }
}
