using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyCursS.Models 
{

    public class Person
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}