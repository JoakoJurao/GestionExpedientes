using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ObtenerTramitePorExpedienteIdUseCase(ITramiteRepository repository)
{
    public ObtenerTramitePorExpedienteIdResponse Ejecutar(ObtenerTramitePorExpedienteIdRequest request)
    {
        List<Tramite> tramites = repository.ObtenerTramitesPorExpedienteId(request.expId).ToList();
        return new ObtenerTramitePorExpedienteIdResponse(tramites);
    }
}
