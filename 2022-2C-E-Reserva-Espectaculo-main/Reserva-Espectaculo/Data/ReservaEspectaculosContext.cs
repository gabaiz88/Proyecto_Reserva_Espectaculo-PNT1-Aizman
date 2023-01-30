using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reserva_Espectaculo.Models;

public class ReservaEspectaculosContext: IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
{
    public ReservaEspectaculosContext(DbContextOptions options): base(options)
	{
	}

    //Metodo para definir el model FuncionCliente muchos a muchos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FuncionCliente>().HasKey(fc => new { fc.FuncionId, fc.ClienteId });

        modelBuilder.Entity<FuncionCliente>()
            .HasOne(fc => fc.Cliente)
            .WithMany(c => c.ClienteFunciones)
            .HasForeignKey(fc => fc.ClienteId);

        modelBuilder.Entity<FuncionCliente>()
            .HasOne(fc => fc.Funcion)
            .WithMany(f => f.ClientesFuncion)
            .HasForeignKey(fc => fc.FuncionId);

        //Modifico la Entidad Identity User para que guarde en Las tablas que yo quiero
        modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        //Relacion Muchos a Muchos
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empleado> Empleados { get; set; }

    public DbSet<Sala> Salas { get; set; }
    public DbSet<TipoSala> TipoSalas { get; set; }
    public DbSet<Funcion> Funciones { get; set; }

    public DbSet<Pelicula> Peliculas { get; set; }

    public DbSet<Telefono> Telefonos { get; set; }

    public DbSet<Direccion> Direcciones { get; set; }

    public DbSet<Reserva> Reservas { get; set; }

    public DbSet<Reserva_Espectaculo.Models.Persona> Persona { get; set; }

    public DbSet<Rol> Roles { get; set; }

    public DbSet<FuncionCliente> FuncionCliente { get; set; }
}
