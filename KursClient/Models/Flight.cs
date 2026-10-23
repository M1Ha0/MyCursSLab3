using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KursClient.Models;

public partial class Flight
{
    public int NumberFlight { get; set; }

    public string Аirplane { get; set; } = null!;

    public string Departure { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public DateOnly? Date { get; set; }

    public TimeOnly TimeDeparture { get; set; }

    public int TimeFlight { get; set; }
    public decimal Price { get; set; }
    //public virtual ICollection<FreePlace> FreePlaces { get; set; } = new List<FreePlace>();
    //public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();
}
