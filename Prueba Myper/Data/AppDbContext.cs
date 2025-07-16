using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


using Prueba_Myper.Models;
using Prueba_Myper.Models.DTO;
using System.Data;

namespace Prueba_Myper.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TrabajadorVista> TrabajadorVista { get; set; }
        public DbSet<DepartamentoDto> Departamentos { get; set; }
        public DbSet<ProvinciaDto> Provincias { get; set; }
        public DbSet<DistritoDto> Distritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Trabajador>().ToTable("Trabajadores");
            modelBuilder.Entity<Departamento>().ToTable("Departamento");
            modelBuilder.Entity<Provincia>().ToTable("Provincia");
            modelBuilder.Entity<Distrito>().ToTable("Distrito");
        }
        public async Task<List<DepartamentoDto>> ObtenerDepartamentosAsync()
        {
            return await this.Set<DepartamentoDto>()
                .FromSqlRaw("EXEC sp_ListarDepartamentos")
                .ToListAsync();
        }

        public async Task<List<ProvinciaDto>> ObtenerProvinciasAsync(int idDepartamento)
        {
            return await this.Set<ProvinciaDto>()
                .FromSqlRaw("EXEC sp_ListarProvincias @p0", idDepartamento)
                .ToListAsync();
        }
        public async Task<List<DistritoDto>> ObtenerDistritoAsync(int idProvincia)
        {
            return await this.Set<DistritoDto>()
                .FromSqlRaw("EXEC sp_ListarDistritos @p0", idProvincia)
                .ToListAsync();
        }
        public async Task CrearTrabajadorAsync(Trabajador trabajador)
        {
            using var connection = Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "sp_CrearTrabajador";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddRange(new[]
            {
            new SqlParameter("@TipoDocumento", trabajador.TipoDocumento ?? (object)DBNull.Value),
            new SqlParameter("@NumeroDocumento", trabajador.NumeroDocumento ?? (object)DBNull.Value),
            new SqlParameter("@Nombres", trabajador.Nombres ?? (object)DBNull.Value),
            new SqlParameter("@Sexo", trabajador.Sexo ?? (object)DBNull.Value),
            new SqlParameter("@IdDepartamento", trabajador.IdDepartamento),
            new SqlParameter("@IdProvincia", trabajador.IdProvincia),
            new SqlParameter("@IdDistrito", trabajador.IdDistrito)
             });

            await command.ExecuteNonQueryAsync();
        }

        public async Task EliminarTrabajadorAsync(int id)
        {
            var param = new SqlParameter("@Id", id);

            await Database.ExecuteSqlRawAsync("EXEC sp_EliminarTrabajador @Id", param);
        }
        public async Task ActualizarTrabajadorAsync(Trabajador t)
        {
            var parameters = new[]
            {
            new SqlParameter("@Id", t.Id),
            new SqlParameter("@TipoDocumento", t.TipoDocumento ?? (object)DBNull.Value),
            new SqlParameter("@NumeroDocumento", t.NumeroDocumento ?? (object)DBNull.Value),
            new SqlParameter("@Nombres", t.Nombres ?? (object)DBNull.Value),
            new SqlParameter("@Sexo", t.Sexo ?? (object)DBNull.Value),
            new SqlParameter("@IdDepartamento", t.IdDepartamento),
            new SqlParameter("@IdProvincia", t.IdProvincia),
            new SqlParameter("@IdDistrito", t.IdDistrito)
          };

            await Database.ExecuteSqlRawAsync("EXEC sp_ActualizarTrabajador @Id, @TipoDocumento, @NumeroDocumento, @Nombres, @Sexo, @IdDepartamento, @IdProvincia, @IdDistrito", parameters);
        }



    }
}
