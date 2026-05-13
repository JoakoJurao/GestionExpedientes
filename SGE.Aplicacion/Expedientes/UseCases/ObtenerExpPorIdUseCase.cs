using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ObtenerExpPorIdUseCase(IExpedienteRepository repository)
{
    public ObtenerExpPorIdResponse Ejecutar(ObtenerExpPorIdRequest request)
    {
        Expediente? exp =  repository.ObtenerExpPorId(request.expId);
        return new ObtenerExpPorIdResponse(exp);
    }
}