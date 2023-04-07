using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Order
{
    public int Idorder { get; set; }

    public DateOnly Date { get; set; }

    public int IdUser { get; set; }

    public int IdStatusOrder { get; set; }

    public virtual StatusOrder IdStatusOrderNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<OrderHelper> OrderHelpers { get; } = new List<OrderHelper>();
}
