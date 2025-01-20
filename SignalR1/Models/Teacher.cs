using System;
using System.Collections.Generic;

namespace SignalR1.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Sts { get; set; }

    public virtual User User { get; set; } = null!;
}
