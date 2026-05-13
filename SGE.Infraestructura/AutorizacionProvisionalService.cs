using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid usuarioId, PermisoEnum permisoRequerido)
    {
        return true;
    }
}