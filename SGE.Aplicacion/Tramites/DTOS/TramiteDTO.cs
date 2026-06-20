using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOS;

public record class TramiteDTO(Guid Id, EtiquetaTramiteEnum Etiqueta, string Contenido, DateTime FechaCreacion, DateTime FechaUltimaModificacion);
