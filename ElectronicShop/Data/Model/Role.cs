using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Role
{
    public int IdRole { get; set; }

    public string Role1 { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
