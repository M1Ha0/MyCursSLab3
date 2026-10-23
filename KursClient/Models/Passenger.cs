using System;
using System.Collections.Generic;

namespace KursClient.Models;

public partial class Passenger
{
    public int IdPassenger { get; set; }

    public string SurName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public int DocumentSerial { get; set; }

    public int DocumentNumber { get; set; }

    public int NumberFlight { get; set; }

}
