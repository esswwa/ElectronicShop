using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Product
{
    public int IdProduct { get; set; }

    public string NameProduct { get; set; } = null!;

    public string? ImgProduct { get; set; }

    public string FirmProduct { get; set; } = null!;

    public int CostProduct { get; set; }

    public int CategoryProduct { get; set; }

    public float? ReitingProduct { get; set; }

    public int CountProduct { get; set; }

    public int Status { get; set; }

    public virtual Category CategoryProductNavigation { get; set; } = null!;

    public virtual ICollection<Favourity> Favourities { get; } = new List<Favourity>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<HelperBasket> HelperBaskets { get; } = new List<HelperBasket>();

    public virtual Status StatusNavigation { get; set; } = null!;
}
