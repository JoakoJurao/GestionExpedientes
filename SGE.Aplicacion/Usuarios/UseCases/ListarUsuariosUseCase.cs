using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios.DTOS;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ListarUsuariosUseCase(
    IUsuarioRepository repository
    )
{
    public ListarUsuariosResponse Ejecutar(Guid idUsuarioAutenticado)
    {
        Usuario admin = repository.ObtenerPorId(idUsuarioAutenticado)
            ?? throw new EntidadNoEncontradaException("Usuario");
        if (!admin.EsAdministrador)
            throw new AutorizacionException("Solo un administrador puede listar usuarios.");

        var usuarios = repository.ObtenerTodos()
            .Select(u => new UsuarioDTO(u.Id, u.Nombre, u.CorreoElectronico, u.EsAdministrador, u.Permisos));
        return new ListarUsuariosResponse(usuarios);
    }
}
