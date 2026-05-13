using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ObtenerTodosExpUseCase(IExpedienteRepository repository)
{
    public ObtenerTodosExpResponse Ejecutar()
    {
        List<Expediente> expedientes = repository.ObtenerTodosExp().ToList();
        return new ObtenerTodosExpResponse(expedientes);
    }
}