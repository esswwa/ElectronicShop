using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Firm
{
    public int Idfirms { get; set; }

    public string Firm1 { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
