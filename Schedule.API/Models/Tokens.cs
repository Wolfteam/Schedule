using System;

namespace Schedule.API.Models
{
    public partial class Tokens
    {
        public uint IdToken { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }

        public Admin Admin { get; set; }
    }
}
