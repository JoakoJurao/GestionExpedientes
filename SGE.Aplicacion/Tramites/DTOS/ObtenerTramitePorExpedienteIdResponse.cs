using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.DTOS;

public record class ObtenerTramitePorExpedienteIdResponse(IEnumerable<Tramite> tramites);
