using SGE.Aplicacion.Expedientes.DTOS;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ObtenerTodosExpUseCase(IExpedienteRepository repository)
{
    public ObtenerTodosExpResponse Ejecutar()
    {
        var expedientes = repository.ObtenerTodosExp()
            .Select(e => new ExpedienteDTO(e.Id, e.Caratula.Contenido, e.Estado, e.FechaCreacion, e.FechaUltimaModificacion));
        return new ObtenerTodosExpResponse(expedientes);
    }
}