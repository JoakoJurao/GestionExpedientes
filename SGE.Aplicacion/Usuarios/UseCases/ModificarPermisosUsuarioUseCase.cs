using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios.DTOS;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ModificarPermisosUsuarioUseCase(
    IUsuarioRepository repository,
    IUnidadDeTrabajo udt
    )
{
    public ModificarPermisosUsuarioResponse Ejecutar(ModificarPermisosUsuarioRequest request, Guid idUsuarioAutenticado)
    {
        Usuario admin = repository.ObtenerPorId(idUsuarioAutenticado)
            ?? throw new EntidadNoEncontradaException("Usuario");
        if (!admin.EsAdministrador)
            throw new AutorizacionException("Solo un administrador puede modificar permisos.");

        Usuario usuario = repository.ObtenerPorId(request.UsuarioIdAModificar)
            ?? throw new EntidadNoEncontradaException("Usuario");

        usuario.Permisos.Clear();
        foreach (PermisoEnum permiso in request.Permisos)
            usuario.AgregarPermiso(permiso);

        udt.Guardar();
        return new ModificarPermisosUsuarioResponse();
    }
}
