﻿using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Order
{
    public int Idorder { get; set; }

    public DateOnly DateOrder { get; set; }

    public int IdUser { get; set; }

    public int IdStatusOrder { get; set; }

    public DateOnly DateReceipt { get; set; }

    public int IdOrderHelper { get; set; }

    public double AllCost { get; set; }

    public virtual StatusOrder IdStatusOrderNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
