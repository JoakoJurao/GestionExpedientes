using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.UseCases;

public class AgregarTramiteUseCase(IAutorizacionService autorizacionService, ITramiteRepository repository, ITimeProvider timeProvider, Expedientes.IExpedienteRepository repository_exp)
{
    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.TramiteAlta))
            throw new AutorizacionException("No posee los permisos necesarios");
        ContenidoTramite contenido = new ContenidoTramite(request.contenido);
        Tramite tramite = Tramite.CrearTramite(request.expId, request.etiqueta, contenido, request.usuarioId, timeProvider.Now);
        repository.AgregarTramite(tramite);
        ActualizacionEstadoExpedienteService a = new ActualizacionEstadoExpedienteService(repository_exp, repository, timeProvider);
        a.Ejecutar(request.expId, request.usuarioId);
        return new AgregarTramiteResponse(tramite.Id);
    }
}