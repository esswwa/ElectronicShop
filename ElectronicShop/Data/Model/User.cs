using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class User
{
    public int Iduser { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string Login { get; set; } = null!;

    public string EmailOrNumberPhone { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Basket> Baskets { get; } = new List<Basket>();

    public virtual ICollection<Favourity> Favourities { get; } = new List<Favourity>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
