using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Favourity
{
    public int Idfavourities { get; set; }

    public int IdProduct { get; set; }

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
