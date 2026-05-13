using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarEtiquetaTramiteUseCase(IAutorizacionService autorizacionService, ITramiteRepository repository, ITimeProvider timeProvider, IExpedienteRepository repository_exp)
{
    public ModificarEtiquetaTramiteResponse Ejecutar(ModificarEtiquetaTramiteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.TramiteModificacion))
            throw new AutorizacionException("No posee los permisos necesarios");
        Tramite t = repository.ObtenerTramitePorId(request.tramiteId)?? throw new AplicacionException("Tramite no encontrado");
        t.ModificarEtiqueta(request.nuevaEtiqueta, request.usuarioId, timeProvider.Now);
        repository.EliminarTramite(request.tramiteId);
        repository.AgregarTramite(t);
        ActualizacionEstadoExpedienteService a = new ActualizacionEstadoExpedienteService(repository_exp, repository,timeProvider);
        a.Ejecutar(t.ExpedienteId, request.usuarioId);
        return new ModificarEtiquetaTramiteResponse(t.Id);
    }
}
