namespace SGE.Aplicacion.Expedientes.DTOS;

public record class ObtenerTodosExpResponse(IEnumerable<ExpedienteDTO> Expedientes);
