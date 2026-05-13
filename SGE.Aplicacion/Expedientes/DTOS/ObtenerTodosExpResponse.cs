using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes.DTOS;

public record class ObtenerTodosExpResponse(IEnumerable<Expediente> expedientes);
