using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class EliminarExpedienteUseCase(
    IAutorizacionService autorizacionService,
    IExpedienteRepository repository,
    ITramiteRepository repositoryTramite,
    IUnidadDeTrabajo udt
    )
{
    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.ExpedienteBaja))
            throw new AutorizacionException("No posee los permisos necesarios");
        if (repository.ObtenerExpPorId(request.expId) is null) throw new EntidadNoEncontradaException("Expediente");
        foreach (Tramite t in repositoryTramite.ObtenerTramitesPorExpedienteId(request.expId))
        {
            repositoryTramite.EliminarTramite(t.Id);
        }
        repository.EliminarExpediente(request.expId);
        udt.Guardar();
        return new EliminarExpedienteResponse();
    }
}
