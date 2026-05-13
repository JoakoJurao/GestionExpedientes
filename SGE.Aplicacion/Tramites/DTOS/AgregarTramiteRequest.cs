using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOS;

public record class AgregarTramiteRequest(Guid expId, EtiquetaTramiteEnum etiqueta, String contenido, Guid usuarioId);
