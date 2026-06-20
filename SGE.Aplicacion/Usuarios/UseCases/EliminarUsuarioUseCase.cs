using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios.DTOS;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class EliminarUsuarioUseCase(
    IUsuarioRepository repository,
    IUnidadDeTrabajo udt
    )
{
    public EliminarUsuarioResponse Ejecutar(EliminarUsuarioRequest request, Guid idUsuarioAutenticado)
    {
        Usuario admin = repository.ObtenerPorId(idUsuarioAutenticado)
            ?? throw new EntidadNoEncontradaException("Usuario");
        if (!admin.EsAdministrador)
            throw new AutorizacionException("Solo un administrador puede eliminar usuarios.");

        if (repository.ObtenerPorId(request.UsuarioIdAEliminar) is null)
            throw new EntidadNoEncontradaException("Usuario");

        repository.EliminarUsuario(request.UsuarioIdAEliminar);
        udt.Guardar();
        return new EliminarUsuarioResponse();
    }
}
