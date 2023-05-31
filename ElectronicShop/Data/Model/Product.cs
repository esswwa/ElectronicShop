using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Product
{
    public int IdProduct { get; set; }

    public string NameProduct { get; set; } = null!;

    public int ImgProduct { get; set; }

    public int FirmProduct { get; set; }

    public int CostProduct { get; set; }

    public int CategoryProduct { get; set; }

    public float ReitingProduct { get; set; }

    public int CountProduct { get; set; }

    public int Status { get; set; }

    public string Description { get; set; } = null!;

    public string SecondNameProduct { get; set; } = null!;

    public string Article { get; set; } = null!;


    public virtual Category CategoryProductNavigation { get; set; } = null!;

    public virtual ICollection<Favourity> Favourities { get; } = new List<Favourity>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<HelperBasket> HelperBaskets { get; } = new List<HelperBasket>();

    public virtual ICollection<OrderHelper> OrderHelpers { get; } = new List<OrderHelper>();

    public virtual Status StatusNavigation { get; set; } = null!;

    public virtual Firms FirmsNavigation { get; set; } = null!;

}
