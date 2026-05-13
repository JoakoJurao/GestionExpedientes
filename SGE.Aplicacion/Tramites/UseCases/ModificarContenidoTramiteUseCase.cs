using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarContenidoTramiteUseCase(IAutorizacionService autorizacionService, ITramiteRepository repository, ITimeProvider timeProvider)
{
    public ModificarContenidoTramiteResponse Ejecutar(ModificarContenidoTramiteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.TramiteModificacion))
            throw new AutorizacionException("No posee los permisos necesarios");
        Tramite t = repository.ObtenerTramitePorId(request.tramiteId)?? throw new AplicacionException("Tramite no encontrado");
        ContenidoTramite nuevoCont = new ContenidoTramite(request.nuevoContenido);
        t.ModificarContenido(nuevoCont, request.usuarioId, timeProvider.Now);
        repository.EliminarTramite(request.tramiteId);
        repository.AgregarTramite(t);
        return new ModificarContenidoTramiteResponse(t.Id);
    }
}
