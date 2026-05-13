using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOS;

public record class ModificarEtiquetaTramiteRequest(Guid tramiteId, Guid usuarioId, EtiquetaTramiteEnum nuevaEtiqueta);
