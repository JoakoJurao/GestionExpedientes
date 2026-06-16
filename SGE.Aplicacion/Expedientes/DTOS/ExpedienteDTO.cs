using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOS;

public record class ExpedienteDTO(Guid Id, string Caratula, EstadoExpEnum Estado, DateTime FechaCreacion, DateTime FechaUltimaModificacion);
