using Schedule.Entities;
using System.Collections.Generic;

namespace Schedule.Web.ViewModels
{
    public class SideNavViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string IPAdress { get; set; }
        public IEnumerable<Privilegios> Privilegios { get; set; }
    }
}