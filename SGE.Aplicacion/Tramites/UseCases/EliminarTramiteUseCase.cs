using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.UseCases;

public class EliminarTramiteUseCase(IAutorizacionService autorizacionService, ITramiteRepository repository, ITimeProvider timeProvider, Expedientes.IExpedienteRepository repository_exp)
{
    public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.TramiteBaja))
            throw new AutorizacionException("No posee los permisos necesarios");
        Tramite tramite = repository.ObtenerTramitePorId(request.tramiteId) ?? throw new EntidadNoEncontradaException("Tramite");
        repository.EliminarTramite(request.tramiteId);
        ActualizacionEstadoExpedienteService a = new ActualizacionEstadoExpedienteService(repository_exp, repository, timeProvider);
        a.Ejecutar(tramite.ExpedienteId, request.usuarioId);
        return new EliminarTramiteResponse();
    }
}
