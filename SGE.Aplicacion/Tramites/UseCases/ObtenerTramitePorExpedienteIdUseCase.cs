using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Tramites.DTOS;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ObtenerTramitePorExpedienteIdUseCase(
    ITramiteRepository repository
    )
{
    public ObtenerTramitePorExpedienteIdResponse Ejecutar(ObtenerTramitePorExpedienteIdRequest request)
    {
        IEnumerable<TramiteDTO> tramitesDto = repository.ObtenerTramitesPorExpedienteId(request.expId)
            .Select(t => new TramiteDTO(t.Id, t.Etiqueta, t.Contenido.Texto, t.FechaCreacion, t.FechaUltimaModificacion));
        return new ObtenerTramitePorExpedienteIdResponse(tramitesDto);
    }
}
