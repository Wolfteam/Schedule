using Microsoft.EntityFrameworkCore;
using Schedule.Data.Models;

namespace Schedule.Data
{
    public class ScheduleContext : DbContext
    {
        public ScheduleContext(DbContextOptions options) : base(options) { }

        public DbSet<Profesor> Profesores { get; set; }
    }

    /// <summary>
    /// Factory class for EmployeesContext
    /// </summary>
    //public static class EmployeesContextFactory
    //{
    //    public static ScheduleContext Create(string connectionString)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<ScheduleContext>();
    //        optionsBuilder.UseMySQL(connectionString);

    //        //Ensure database creation
    //        var context = new ScheduleContext(optionsBuilder.Options);
    //        context.Database.EnsureCreated();

    //        return context;
    //    }
    //}
}
