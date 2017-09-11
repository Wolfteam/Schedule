using Schedule.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Data
{
    public interface IProfesor
    {
        IEnumerable<Profesor> GetAll();
    }
}
