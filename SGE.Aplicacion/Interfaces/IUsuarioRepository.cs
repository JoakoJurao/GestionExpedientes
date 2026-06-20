using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Interfaces;

public interface IUsuarioRepository
{
    void AgregarUsuario(Usuario usuario);
    void EliminarUsuario(Guid id);
    Usuario? ObtenerPorCorreo(string correoElectronico);
    Usuario? ObtenerPorId(Guid id);
    IEnumerable<Usuario> ObtenerTodos();
}
