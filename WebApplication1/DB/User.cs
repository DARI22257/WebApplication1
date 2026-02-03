using System;
using System.Collections.Generic;

namespace WebApplication1.DB;

public partial class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Nickname { get; set; }
}
