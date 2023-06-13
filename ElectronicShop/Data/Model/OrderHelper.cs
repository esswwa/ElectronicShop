using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class OrderHelper
{
    public int IdorderHelper { get; set; }

    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public int Count { get; set; }

    public double Cost { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;
}
