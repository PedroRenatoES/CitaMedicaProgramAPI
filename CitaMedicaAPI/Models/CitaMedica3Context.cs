using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CitaMedicaAPI.Models;

public partial class CitaMedica3Context : DbContext
{
    public CitaMedica3Context()
    {
    }

    public CitaMedica3Context(DbContextOptions<CitaMedica3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Alergia> Alergias { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Especialidade> Especialidades { get; set; }

    public virtual DbSet<HistorialHorario> HistorialHorarios { get; set; }

    public virtual DbSet<HistorialMedico> HistorialMedicos { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Medicamento> Medicamentos { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<PruebasDiagnostica> PruebasDiagnosticas { get; set; }

    public virtual DbSet<Receta> Recetas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server='DESKTOP-KM3OB2Q\\SQLEXPRESS'; User Id='DESKTOP-KM3OB2Q\\Tato'; Password='';Database=citaMedica3;Trusted_Connection=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alergia>(entity =>
        {
            entity.HasKey(e => e.IdAlergia).HasName("PK__Alergias__874F21E5AE83EF95");

            entity.Property(e => e.IdAlergia).HasColumnName("ID_Alergia");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.IdPaciente).HasColumnName("ID_Paciente");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Alergia)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("FK__Alergias__ID_Pac__2E1BDC42");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Citas__7C17FD1690FA63FE");

            entity.Property(e => e.IdCita).HasColumnName("ID_Cita");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdMedico).HasColumnName("ID_Medico");
            entity.Property(e => e.IdPaciente).HasColumnName("ID_Paciente");
            entity.Property(e => e.Motivo).HasMaxLength(200);

            entity.HasOne(d => d.IdMedicoNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdMedico)
                .HasConstraintName("FK__Citas__ID_Medico__20C1E124");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("FK__Citas__ID_Pacien__1FCDBCEB");
        });

        modelBuilder.Entity<Especialidade>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("PK__Especial__5D7732D7E3668B87");

            entity.HasIndex(e => e.Nombre, "UQ__Especial__75E3EFCFDF29A714").IsUnique();

            entity.Property(e => e.IdEspecialidad).HasColumnName("ID_Especialidad");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<HistorialHorario>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__Historia__ECA894545CB8C645");

            entity.ToTable("Historial_Horarios");

            entity.Property(e => e.IdHistorial).HasColumnName("ID_Historial");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Modificacion");
            entity.Property(e => e.IdHorario).HasColumnName("ID_Horario");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.HistorialHorarios)
                .HasForeignKey(d => d.IdHorario)
                .HasConstraintName("FK__Historial__ID_Ho__33D4B598");
        });

        modelBuilder.Entity<HistorialMedico>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__Historia__ECA8945489799B66");

            entity.ToTable("Historial_Medico");

            entity.Property(e => e.IdHistorial).HasColumnName("ID_Historial");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Diagnostico).HasMaxLength(200);
            entity.Property(e => e.IdPaciente).HasColumnName("ID_Paciente");
            entity.Property(e => e.Tratamiento).HasMaxLength(200);

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.HistorialMedicos)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("FK__Historial__ID_Pa__24927208");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horarios__77A009BD0C5E5AB3");

            entity.Property(e => e.IdHorario).HasColumnName("ID_Horario");
            entity.Property(e => e.Dia).HasMaxLength(10);
            entity.Property(e => e.HoraFin).HasColumnName("Hora_Fin");
            entity.Property(e => e.HoraInicio).HasColumnName("Hora_Inicio");
            entity.Property(e => e.IdMedico).HasColumnName("ID_Medico");

            entity.HasOne(d => d.IdMedicoNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdMedico)
                .HasConstraintName("FK__Horarios__ID_Med__1CF15040");
        });

        modelBuilder.Entity<Medicamento>(entity =>
        {
            entity.HasKey(e => e.IdMedicamento).HasName("PK__Medicame__C1C5A0423B0C6802");

            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Dosis).HasMaxLength(50);
            entity.Property(e => e.EfectosSecundarios)
                .HasMaxLength(200)
                .HasColumnName("Efectos_Secundarios");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.IdMedico).HasName("PK__Medicos__EFBF88F7F573B94E");

            entity.Property(e => e.IdMedico).HasColumnName("ID_Medico");
            entity.Property(e => e.IdEspecialidad).HasColumnName("ID_Especialidad");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

            entity.HasOne(d => d.IdEspecialidadNavigation).WithMany(p => p.Medicos)
                .HasForeignKey(d => d.IdEspecialidad)
                .HasConstraintName("FK__Medicos__ID_Espe__1920BF5C");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Medicos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Medicos__ID_Usua__1A14E395");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.IdPaciente).HasName("PK__Paciente__5F36506146F09DA1");

            entity.Property(e => e.IdPaciente).HasColumnName("ID_Paciente");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Pacientes__ID_Us__164452B1");
        });

        modelBuilder.Entity<PruebasDiagnostica>(entity =>
        {
            entity.HasKey(e => e.IdPrueba).HasName("PK__Pruebas___FCB047EE58BE2311");

            entity.ToTable("Pruebas_Diagnosticas");

            entity.Property(e => e.IdPrueba).HasColumnName("ID_Prueba");
            entity.Property(e => e.IdPaciente).HasColumnName("ID_Paciente");
            entity.Property(e => e.TipoPrueba)
                .HasMaxLength(100)
                .HasColumnName("Tipo_Prueba");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.PruebasDiagnosticas)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("FK__Pruebas_D__ID_Pa__30F848ED");
        });

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.HasKey(e => e.IdReceta).HasName("PK__Recetas__19C046318961A6C3");

            entity.Property(e => e.IdReceta).HasColumnName("ID_Receta");
            entity.Property(e => e.Dosis).HasMaxLength(50);
            entity.Property(e => e.Duracion).HasMaxLength(50);
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Fecha_Emision");
            entity.Property(e => e.IdHistorial).HasColumnName("ID_Historial");
            entity.Property(e => e.IdMedicamento).HasColumnName("ID_Medicamento");

            entity.HasOne(d => d.IdHistorialNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdHistorial)
                .HasConstraintName("FK__Recetas__ID_Hist__29572725");

            entity.HasOne(d => d.IdMedicamentoNavigation).WithMany(p => p.Receta)
                .HasForeignKey(d => d.IdMedicamento)
                .HasConstraintName("FK__Recetas__ID_Medi__2A4B4B5E");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__DE4431C548B0D265");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534DE367733").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.ContraseñaHash)
                .HasMaxLength(200)
                .HasColumnName("Contraseña_Hash");
            entity.Property(e => e.ContraseñaSalt)
                .HasMaxLength(200)
                .HasColumnName("Contraseña_Salt");
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaNacimiento).HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.Genero).HasMaxLength(10);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(10);
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
