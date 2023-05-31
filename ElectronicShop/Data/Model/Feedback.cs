using System;
using System.Collections.Generic;

namespace ElectronicShop.Data.Model;

public partial class Feedback
{
    public int Idfeedback { get; set; }

    public int IdProduct { get; set; }

    public int IdUser { get; set; }

    public string Feedbacks { get; set; } = null!;

    public int Reiting { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
