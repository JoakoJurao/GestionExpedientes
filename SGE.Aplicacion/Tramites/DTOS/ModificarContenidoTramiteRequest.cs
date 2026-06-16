using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOS;

public record class ModificarContenidoTramiteRequest(Guid tramiteId, Guid usuarioId, string nuevoContenido, EtiquetaTramiteEnum nuevaEtiqueta);
