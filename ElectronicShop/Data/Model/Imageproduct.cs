using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Imageproduct
{
    public int IdimageProduct { get; set; }

    public string IdProduct { get; set; } = null!;

    public string Image { get; set; } = null!;
}
