using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;


public partial class Status
{
    public int Idstatus { get; set; }

    public string Status1 { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
