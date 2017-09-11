using Schedule.Data;
using Schedule.Data.Models;
using System;
using System.Collections.Generic;

namespace Schedule.Services
{
    public class ProfesoresService : IProfesor
    {
        private ScheduleContext _context;

        public ProfesoresService(ScheduleContext context)
        {
            _context = context;
        }
        public IEnumerable<Profesor> GetAll()
        {
            IList<Profesor> someTypeList = new List<Profesor>();
            someTypeList = _context.LoadStoredProc("sp_Prueba").ExecuteStoredProc<Profesor>();
            return someTypeList;
        }
    }
}
