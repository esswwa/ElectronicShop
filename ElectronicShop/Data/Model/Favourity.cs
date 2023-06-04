using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Favourity
{
    public int Idfavourities { get; set; }

    public string IdProduct { get; set; } = null!;

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
