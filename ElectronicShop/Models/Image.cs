using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Models
{
    public class Image
    {
        public string Images { get; set; }
        string image { get { return Path.GetFullPath($@"Resources\Image\{Images}"); } }
    }
}
