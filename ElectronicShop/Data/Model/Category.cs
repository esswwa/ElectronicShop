using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;


public partial class Category
{
    public int Idcategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategoryNameDeep { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
