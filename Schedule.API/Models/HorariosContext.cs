using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Schedule.Entities;
using Microsoft.Extensions.Options;

namespace Schedule.API.Models
{
    public partial class HorariosContext : DbContext
    {
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Aulas> Aulas { get; set; }
        public virtual DbSet<Carreras> Carreras { get; set; }
        public virtual DbSet<Dias> Dias { get; set; }
        public virtual DbSet<DisponibilidadProfesores> DisponibilidadProfesores { get; set; }
        public virtual DbSet<HorarioProfesores> HorarioProfesores { get; set; }
        public virtual DbSet<Horas> Horas { get; set; }
        public virtual DbSet<Materias> Materias { get; set; }
        public virtual DbSet<PeriodoCarrera> PeriodoCarrera { get; set; }
        public virtual DbSet<PrioridadProfesor> PrioridadProfesor { get; set; }
        public virtual DbSet<Privilegios> Privilegios { get; set; }
        public virtual DbSet<Profesores> Profesores { get; set; }
        public virtual DbSet<ProfesoresMaterias> ProfesoresMaterias { get; set; }
        public virtual DbSet<Secciones> Secciones { get; set; }
        public virtual DbSet<Semestres> Semestre { get; set; }
        public virtual DbSet<TipoAsignacion> TipoAsignacion { get; set; }
        public virtual DbSet<TipoAulaMaterias> TipoAulaMateria { get; set; }
        public virtual DbSet<Tokens> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseMySql(_appSetting.Value.ConnectionString);
                #warning  Debo quitar la connection string de aca
                optionsBuilder.UseMySql("server=localhost;userid=wolfteam20;pwd=sistemas;port=3306;database=horarios;sslmode=none;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Cedula);

                entity.ToTable("admin");

                entity.HasIndex(e => e.IdPrivilegio)
                    .HasName("id_privilegio");

                entity.HasIndex(e => e.Username)
                    .HasName("username")
                    .IsUnique();

                entity.Property(e => e.Cedula)
                    .HasColumnName("cedula")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdPrivilegio).HasColumnName("id_privilegio");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(20);

                entity.HasOne(d => d.CedulaNavigation)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.Cedula)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("admin_ibfk_1");

                entity.HasOne(d => d.IdPrivilegioNavigation)
                    .WithMany(p => p.Admin)
                    .HasForeignKey(d => d.IdPrivilegio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("admin_ibfk_2");
            });

            modelBuilder.Entity<Aulas>(entity =>
            {
                entity.HasKey(e => e.IdAula);

                entity.ToTable("aulas");

                entity.HasIndex(e => e.IdTipo)
                    .HasName("id_tipo");

                entity.Property(e => e.IdAula).HasColumnName("id_aula");

                entity.Property(e => e.Capacidad).HasColumnName("capacidad");

                entity.Property(e => e.IdTipo).HasColumnName("id_tipo");

                entity.Property(e => e.NombreAula)
                    .IsRequired()
                    .HasColumnName("nombre_aula")
                    .HasMaxLength(35);

                entity.HasOne(d => d.TipoAulaMateria)
                    .WithMany(p => p.Aulas)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("aulas_ibfk_1");
            });

            modelBuilder.Entity<Carreras>(entity =>
            {
                entity.HasKey(e => e.IdCarrera);

                entity.ToTable("carreras");

                entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");

                entity.Property(e => e.NombreCarrera)
                    .IsRequired()
                    .HasColumnName("nombre_carrera")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Dias>(entity =>
            {
                entity.HasKey(e => e.IdDia);

                entity.ToTable("dias");

                entity.Property(e => e.IdDia).HasColumnName("id_dia");

                entity.Property(e => e.NombreDia)
                    .IsRequired()
                    .HasColumnName("nombre_dia")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<DisponibilidadProfesores>(entity =>
            {
                entity.HasKey(e => new { e.Cedula, e.IdDia, e.IdHoraInicio, e.IdHoraFin });

                entity.ToTable("disponibilidad_profesores");

                entity.HasIndex(e => e.IdDia)
                    .HasName("id_dia");

                entity.HasIndex(e => e.IdHoraFin)
                    .HasName("id_hora_fin");

                entity.HasIndex(e => e.IdHoraInicio)
                    .HasName("id_hora_inicio");

                entity.HasIndex(e => e.IdPeriodo)
                                    .HasName("id_periodo");

                entity.Property(e => e.Cedula).HasColumnName("cedula");

                entity.Property(e => e.IdDia).HasColumnName("id_dia");

                entity.Property(e => e.IdHoraInicio).HasColumnName("id_hora_inicio");

                entity.Property(e => e.IdHoraFin).HasColumnName("id_hora_fin");

                entity.Property(e => e.IdPeriodo)
                                    .HasColumnName("id_periodo")
                                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Profesores)
                    .WithMany(p => p.DisponibilidadProfesores)
                    .HasForeignKey(d => d.Cedula)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("disponibilidad_profesores_ibfk_1");

                entity.HasOne(d => d.Dias)
                    .WithMany(p => p.DisponibilidadProfesores)
                    .HasForeignKey(d => d.IdDia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("disponibilidad_profesores_ibfk_2");

                entity.HasOne(d => d.HoraFin)
                    .WithMany(p => p.DisponibilidadProfesoresIdHoraFinNavigation)
                    .HasForeignKey(d => d.IdHoraFin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("disponibilidad_profesores_ibfk_4");

                entity.HasOne(d => d.HoraInicio)
                    .WithMany(p => p.DisponibilidadProfesoresIdHoraInicioNavigation)
                    .HasForeignKey(d => d.IdHoraInicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("disponibilidad_profesores_ibfk_3");

                entity.HasOne(d => d.PeriodoCarrera)
                    .WithMany(p => p.DisponibilidadProfesores)
                    .HasForeignKey(d => d.IdPeriodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("disponibilidad_profesores_ibfk_5");
            });

            modelBuilder.Entity<HorarioProfesores>(entity =>
            {
                entity.HasKey(e => new { e.Cedula, e.Codigo, e.IdAula, e.IdDia, e.IdHoraInicio, e.IdHoraFin });

                entity.ToTable("horario_profesores");

                entity.HasIndex(e => e.Codigo)
                    .HasName("codigo");

                entity.HasIndex(e => e.IdAula)
                    .HasName("id_aula");

                entity.HasIndex(e => e.IdDia)
                    .HasName("id_dia");

                entity.HasIndex(e => e.IdHoraFin)
                    .HasName("id_hora_fin");

                entity.HasIndex(e => e.IdHoraInicio)
                    .HasName("id_hora_inicio");
                entity.HasIndex(e => e.IdPeriodo)
                    .HasName("id_periodo");
                entity.Property(e => e.Cedula).HasColumnName("cedula");

                entity.Property(e => e.Codigo).HasColumnName("codigo");

                entity.Property(e => e.IdAula).HasColumnName("id_aula");

                entity.Property(e => e.IdDia).HasColumnName("id_dia");

                entity.Property(e => e.IdHoraInicio).HasColumnName("id_hora_inicio");

                entity.Property(e => e.IdHoraFin).HasColumnName("id_hora_fin");
                
                entity.Property(e => e.IdPeriodo)
                    .HasColumnName("id_periodo")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdTipoAsignacion)
                    .HasColumnName("id_asignacion")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NumeroSeccion).HasColumnName("numero_seccion");

                entity.HasOne(d => d.TipoAsignacion)
                    .WithMany(p => p.HorarioProfesores)
                    .HasForeignKey(d => d.IdTipoAsignacion)
                    .HasConstraintName("horario_profesores_ibfk_8");

                entity.HasOne(d => d.Profesores)
                    .WithMany(p => p.HorarioProfesores)
                    .HasForeignKey(d => d.Cedula)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("horario_profesores_ibfk_1");

                entity.HasOne(d => d.Secciones)
                    .WithMany(p => p.HorarioProfesores)
                    .HasForeignKey(d => d.Codigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("horario_profesores_ibfk_2");

                entity.HasOne(d => d.Aulas)
                    .WithMany(p => p.HorarioProfesores)
                    .HasForeignKey(d => d.IdAula)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("horario_profesores_ibfk_6");

                entity.HasOne(d => d.Dias)
                    .WithMany(p => p.HorarioProfesores)
                    .HasForeignKey(d => d.IdDia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("horario_profesores_ibfk_3");

                entity.HasOne(d => d.HoraFin)
                    .WithMany(p => p.HorarioProfesoresIdHoraFinNavigation)
                    .HasForeignKey(d => d.IdHoraFin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("horario_profesores_ibfk_5");

                entity.HasOne(d => d.HoraInicio)
                    .WithMany(p => p.HorarioProfesoresIdHoraInicioNavigation)
                    .HasForeignKey(d => d.IdHoraInicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("horario_profesores_ibfk_4");
                entity.HasOne(d => d.PeriodoCarrera)
                    .WithMany(p => p.HorarioProfesores)
                    .HasForeignKey(d => d.IdPeriodo)
                    .HasConstraintName("horario_profesores_ibfk_7");
            });

            modelBuilder.Entity<Horas>(entity =>
            {
                entity.HasKey(e => e.IdHora);

                entity.ToTable("horas");

                entity.Property(e => e.IdHora).HasColumnName("id_hora");

                entity.Property(e => e.NombreHora)
                    .IsRequired()
                    .HasColumnName("nombre_hora")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Materias>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("materias");

                entity.HasIndex(e => e.IdCarrera)
                    .HasName("id_carrera");

                entity.HasIndex(e => e.IdSemestre)
                    .HasName("id_semestre");

                entity.HasIndex(e => e.IdTipo)
                    .HasName("id_tipo");

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo")
                    .ValueGeneratedNever();

                entity.Property(e => e.Asignatura)
                    .IsRequired()
                    .HasColumnName("asignatura")
                    .HasMaxLength(40);

                entity.Property(e => e.HorasAcademicasSemanales).HasColumnName("horas_academicas_semanales");

                entity.Property(e => e.HorasAcademicasTotales).HasColumnName("horas_academicas_totales");

                entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");

                entity.Property(e => e.IdSemestre).HasColumnName("id_semestre");

                entity.Property(e => e.IdTipo).HasColumnName("id_tipo");

                entity.HasOne(d => d.Carreras)
                    .WithMany(p => p.Materias)
                    .HasForeignKey(d => d.IdCarrera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("materias_ibfk_3");

                entity.HasOne(d => d.Semestres)
                    .WithMany(p => p.Materias)
                    .HasForeignKey(d => d.IdSemestre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("materias_ibfk_1");

                entity.HasOne(d => d.TipoAulaMaterias)
                    .WithMany(p => p.Materias)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("materias_ibfk_2");
            });

            modelBuilder.Entity<PeriodoCarrera>(entity =>
            {
                entity.HasKey(e => e.IdPeriodo);

                entity.ToTable("periodo_carrera");

                entity.Property(e => e.IdPeriodo)
                    .HasColumnName("id_periodo")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.NombrePeriodo)
                    .IsRequired()
                    .HasColumnName("nombre_periodo")
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("bit(1)");
            });

            modelBuilder.Entity<PrioridadProfesor>(entity =>
            {
                entity.HasKey(e => e.IdPrioridad);

                entity.ToTable("prioridad_profesor");

                entity.Property(e => e.IdPrioridad).HasColumnName("id_prioridad");

                entity.Property(e => e.CodigoPrioridad)
                    .IsRequired()
                    .HasColumnName("codigo_prioridad")
                    .HasMaxLength(10);

                entity.Property(e => e.HorasACumplir).HasColumnName("horas_a_cumplir");
            });

            modelBuilder.Entity<Privilegios>(entity =>
            {
                entity.HasKey(e => e.IdPrivilegio);

                entity.ToTable("privilegios");

                entity.Property(e => e.IdPrivilegio).HasColumnName("id_privilegio");

                entity.Property(e => e.NombrePrivilegio)
                    .IsRequired()
                    .HasColumnName("nombre_privilegio")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Profesores>(entity =>
            {
                entity.HasKey(e => e.Cedula);

                entity.ToTable("profesores");

                entity.HasIndex(e => e.IdPrioridad)
                    .HasName("id_prioridad");

                entity.Property(e => e.Cedula)
                    .HasColumnName("cedula")
                    .ValueGeneratedNever();

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasColumnName("apellido")
                    .HasMaxLength(30);

                entity.Property(e => e.Apellido2)
                    .HasColumnName("apellido2")
                    .HasMaxLength(30);

                entity.Property(e => e.IdPrioridad).HasColumnName("id_prioridad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(30);

                entity.Property(e => e.Nombre2)
                    .HasColumnName("nombre2")
                    .HasMaxLength(30);

                entity.HasOne(d => d.PrioridadProfesor)
                    .WithMany(p => p.Profesores)
                    .HasForeignKey(d => d.IdPrioridad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profesores_ibfk_1");
            });

            modelBuilder.Entity<ProfesoresMaterias>(entity =>
            {
                entity.HasKey(e => new { e.Cedula, e.Codigo });

                entity.ToTable("profesores_materias");

                entity.HasIndex(e => e.Codigo)
                    .HasName("codigo");

                entity.HasIndex(e => e.Id)
                    .HasName("id");

                entity.Property(e => e.Cedula).HasColumnName("cedula");

                entity.Property(e => e.Codigo).HasColumnName("codigo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Profesores)
                    .WithMany(p => p.ProfesoresMaterias)
                    .HasForeignKey(d => d.Cedula)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profesores_materias_ibfk_1");

                entity.HasOne(d => d.Materias)
                    .WithMany(p => p.ProfesoresMaterias)
                    .HasForeignKey(d => d.Codigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profesores_materias_ibfk_2");
            });

            modelBuilder.Entity<Secciones>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("secciones");

                entity.HasIndex(e => e.IdPeriodo)
                                    .HasName("id_periodo");

                entity.Property(e => e.Codigo)
                    .HasColumnName("codigo")
                    .ValueGeneratedNever();

                entity.Property(e => e.CantidadAlumnos).HasColumnName("cantidad_alumnos");

                entity.Property(e => e.IdPeriodo)
                                    .HasColumnName("id_periodo")
                                    .HasColumnType("int(11)");

                entity.Property(e => e.NumeroSecciones).HasColumnName("numero_secciones");

                entity.HasOne(d => d.Materias)
                    .WithOne(p => p.Secciones)
                    .HasForeignKey<Secciones>(d => d.Codigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("secciones_ibfk_1");

                entity.HasOne(d => d.PeriodoCarrera)
                    .WithMany(p => p.Secciones)
                    .HasForeignKey(d => d.IdPeriodo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("secciones_ibfk_2");
            });

            modelBuilder.Entity<Semestres>(entity =>
            {
                entity.HasKey(e => e.IdSemestre);

                entity.ToTable("semestre");

                entity.Property(e => e.IdSemestre).HasColumnName("id_semestre");

                entity.Property(e => e.NombreSemestre)
                    .IsRequired()
                    .HasColumnName("nombre_semestre")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TipoAsignacion>(entity =>
            {
                entity.HasKey(e => e.IdAsignacion);

                entity.ToTable("tipo_asignacion");

                entity.Property(e => e.IdAsignacion)
                    .HasColumnName("id_asignacion")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.NombreAsignacion)
                    .IsRequired()
                    .HasColumnName("nombre_asignacion")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TipoAulaMaterias>(entity =>
            {
                entity.HasKey(e => e.IdTipo);

                entity.ToTable("tipo_aula_materia");

                entity.Property(e => e.IdTipo).HasColumnName("id_tipo");

                entity.Property(e => e.NombreTipo)
                    .IsRequired()
                    .HasColumnName("nombre_tipo")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Tokens>(entity =>
            {
                entity.HasKey(e => e.IdToken);

                entity.ToTable("tokens");

                entity.HasIndex(e => e.Token)
                    .HasName("token")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("username");

                entity.Property(e => e.IdToken)
                    .HasColumnName("id_token")
                    .ValueGeneratedNever();

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fecha_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaExpiracion)
                    .HasColumnName("fecha_expiracion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token")
                    .HasMaxLength(255);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(20);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Tokens)
                    .HasPrincipalKey(p => p.Username)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tokens_ibfk_1");
            });
        }
    }
}
