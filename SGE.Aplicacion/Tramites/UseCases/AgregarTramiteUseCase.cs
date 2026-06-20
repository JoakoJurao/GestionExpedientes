using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class AgregarTramiteUseCase(
    IAutorizacionService autorizacionService,
    ITramiteRepository repository,
    ITimeProvider timeProvider,
    IExpedienteRepository repository_exp,
    IUnidadDeTrabajo udt
    )
{
    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.TramiteAlta))
            throw new AutorizacionException("No posee los permisos necesarios");
        ContenidoTramite contenido = new ContenidoTramite(request.contenido);
        Tramite tramite = new Tramite(request.expId, request.etiqueta, contenido, request.usuarioId, timeProvider.Now);
        repository.AgregarTramite(tramite);
        ActualizacionEstadoExpedienteService a = new ActualizacionEstadoExpedienteService(repository_exp, repository, timeProvider);
        a.Ejecutar(request.expId, request.usuarioId);
        udt.Guardar();
        return new AgregarTramiteResponse(tramite.Id);
    }
}
