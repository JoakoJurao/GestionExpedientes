using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class AgregarExpedienteUseCase(
    IAutorizacionService autorizacionService,
    IExpedienteRepository repository,
    Dominio.Comun.ITimeProvider timeProvider,
    IUnidadDeTrabajo udt
    )
{
    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        if (!autorizacionService.PoseeElPermiso(request.usuarioId, PermisoEnum.ExpedienteAlta))
            throw new AutorizacionException("No posee los permisos necesarios");
        CaratulaExp caratula = new CaratulaExp(request.caratulaString);
        Expediente NuevoExpediente = new Expediente(caratula, request.usuarioId, timeProvider.Now);
        repository.AgregarExpediente(NuevoExpediente);
        udt.Guardar();
        return new AgregarExpedienteResponse(NuevoExpediente.Id);
    }
}
