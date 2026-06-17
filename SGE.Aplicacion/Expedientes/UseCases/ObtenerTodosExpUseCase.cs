using SGE.Aplicacion.Expedientes.DTOS;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ObtenerTodosExpUseCase(IExpedienteRepository repository)
{
    public ObtenerTodosExpResponse Ejecutar()
    {
        var expedientes = new List<ExpedienteDTO>();
        foreach (var e in repository.ObtenerTodosExp())
            expedientes.Add(new ExpedienteDTO(e.Id, e.Caratula.Contenido, e.Estado, e.FechaCreacion, e.FechaUltimaModificacion));
        return new ObtenerTodosExpResponse(expedientes);
    }
}