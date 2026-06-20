using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura;

public class AutorizacionService(IUsuarioRepository repository) : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, PermisoEnum permisoRequerido)
    {
        Usuario? usuario = repository.ObtenerPorId(idUsuario);
        if (usuario is null) return false;
        if (usuario.EsAdministrador) return true;

        if (usuario.TienePermiso(permisoRequerido)) return true;

        if (permisoRequerido == PermisoEnum.TramiteBaja && usuario.TienePermiso(PermisoEnum.ExpedienteBaja))
            return true;

        return false;
    }
}
