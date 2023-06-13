using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class HelperBasket
{
    public int IdhelperBasket { get; set; }

    public int IdBasket { get; set; }

    public int IdProduct { get; set; }

    public int Count { get; set; }

    public double Cost { get; set; }

    public virtual Basket IdBasketNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
