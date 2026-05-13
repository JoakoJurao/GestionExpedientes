using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Dominio.Expedientes;


namespace SGE.Aplicacion.Expedientes.UseCases;

public class CambiarEstadoExpedienteUseCase(IAutorizacionService autorizacionService, IExpedienteRepository repository, Dominio.Comun.ITimeProvider timeProvider)
{
    public CambiarEstadoExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.ExpedienteModificacion))
            throw new AutorizacionException("No posee los permisos necesarios");
        Expediente exp = repository.ObtenerExpPorId(request.expId) ?? throw new AplicacionException("Expediente no encontrado");
        exp.CambiarEstado(request.estado, request.usuarioId, timeProvider.Now);
        repository.EliminarExpediente(request.expId);
        repository.AgregarExpediente(exp);
        return new CambiarEstadoExpedienteResponse();
    }
}