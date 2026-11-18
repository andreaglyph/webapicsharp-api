using System.Data;
using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioConsultasPostgreSQL : IRepositorioConsultas
    {
        public Task<DataTable> EjecutarConsultaParametrizadaAsync(string sql, List<SqlParameter>? parametros)
        {
            // TODO: lógica real PostgreSQL
            return Task.FromResult(new DataTable());
        }

        public Task<(bool esValida, string? mensajeError)> ValidarConsultaAsync(string sql, List<SqlParameter>? parametros)
        {
            return Task.FromResult<(bool, string?)>((true, null));
        }

        public Task<DataTable> EjecutarProcedimientoAlmacenadoAsync(string nombre, List<SqlParameter>? parametros)
        {
            return Task.FromResult(new DataTable());
        }

        public Task<string?> ObtenerEsquemaTablaAsync(string esquema, string tabla)
        {
            return Task.FromResult<string?>(null);
        }

        public Task<DataTable> ObtenerEstructuraTablaAsync(string esquema, string tabla)
        {
            return Task.FromResult(new DataTable());
        }

        public Task<DataTable> ObtenerEstructuraBaseDatosAsync(string? nombreBD)
        {
            return Task.FromResult(new DataTable());
        }
    }
}
