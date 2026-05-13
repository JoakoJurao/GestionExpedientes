namespace SGE.Aplicacion.Expedientes.DTOS;

public record class ModificarCaratulaExpedienteRequest(Guid expId, String caratulaString, Guid usuarioId);
