using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Basket
{
    public int Idbasket { get; set; }

    public int IdUser { get; set; }

    public int AllCount { get; set; }

    public int AllCost { get; set; }

    public virtual ICollection<HelperBasket> HelperBaskets { get; } = new List<HelperBasket>();

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
