using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Interfaces;

public interface ITokenProvider
{
    string GenerarToken(Usuario usuario);
}
