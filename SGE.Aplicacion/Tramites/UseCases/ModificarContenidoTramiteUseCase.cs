using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarContenidoTramiteUseCase(IAutorizacionService autorizacionService, ITramiteRepository repository, ITimeProvider timeProvider, IExpedienteRepository repository_exp)
{
    public ModificarContenidoTramiteResponse Ejecutar(ModificarContenidoTramiteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.TramiteModificacion))
            throw new AutorizacionException("No posee los permisos necesarios");
        Tramite t = repository.ObtenerTramitePorId(request.tramiteId) ?? throw new EntidadNoEncontradaException("Tramite");
        ContenidoTramite nuevoCont = new ContenidoTramite(request.nuevoContenido);
        t.ModificarContenido(nuevoCont, request.usuarioId, timeProvider.Now);
        t.ModificarEtiqueta(request.nuevaEtiqueta, request.usuarioId, timeProvider.Now);
        ActualizacionEstadoExpedienteService a = new ActualizacionEstadoExpedienteService(repository_exp, repository,timeProvider);
        a.Ejecutar(t.ExpedienteId, request.usuarioId);
        repository.EliminarTramite(request.tramiteId);
        repository.AgregarTramite(t);
        return new ModificarContenidoTramiteResponse(t.Id);
    }
}
