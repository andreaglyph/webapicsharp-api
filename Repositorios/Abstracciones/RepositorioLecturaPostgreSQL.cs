using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioLecturaPostgreSQL : IRepositorioLecturaTabla
    {
        public Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerFilasAsync(string tabla, string? esquema, int? limite)
            => Task.FromResult<IReadOnlyList<Dictionary<string, object?>>>(new List<Dictionary<string, object?>>());

        public Task<IReadOnlyList<Dictionary<string, object?>>> ObtenerPorClaveAsync(string tabla, string? esquema, string campo, string valor)
            => Task.FromResult<IReadOnlyList<Dictionary<string, object?>>>(new List<Dictionary<string, object?>>());

        public Task<bool> CrearAsync(string tabla, string? esquema, Dictionary<string, object?> datos, string? clavePrimaria)
            => Task.FromResult(false);

        public Task<int> ActualizarAsync(string tabla, string? esquema, string campo, string valor, Dictionary<string, object?> datos, string? clavePrimaria)
            => Task.FromResult(0);

        public Task<int> EliminarAsync(string tabla, string? esquema, string campo, string valor)
            => Task.FromResult(0);

        public Task<string?> ObtenerHashContrasenaAsync(string tabla, string? esquema, string usuario, string campo, string hash)
            => Task.FromResult<string?>(null);
    }
}
