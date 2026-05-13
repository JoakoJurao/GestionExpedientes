using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOS;

public record class CambiarEstadoExpedienteRequest(Guid expId, EstadoExpEnum estado, Guid usuarioId);
