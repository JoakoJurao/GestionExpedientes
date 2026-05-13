using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.UseCases;

public class ObtenerTramitePorIdUseCase(ITramiteRepository repository)
{
    public ObtenerTramitePorIdResponse Ejecutar(ObtenerTramitePorIdRequest request)
    {
        Tramite? t = repository.ObtenerTramitePorId(request.tramiteId);
        return new ObtenerTramitePorIdResponse(t);
    }
}
