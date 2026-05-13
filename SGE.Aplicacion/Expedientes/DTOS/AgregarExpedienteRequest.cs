using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Expedientes.DTOS;

public record class AgregarExpedienteRequest(String caratulaString, Guid usuarioId);
