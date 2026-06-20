using SGE.Aplicacion.Tramites.DTOS;

namespace SGE.Aplicacion.Expedientes.DTOS;

public record class ObtenerExpedientePorIdResponse(ExpedienteDTO Expediente, IEnumerable<TramiteDTO> Tramites);
