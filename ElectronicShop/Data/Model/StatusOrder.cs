using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;


public partial class StatusOrder
{
    public int IdstatusOrder { get; set; }

    public string NameStatus { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
