using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Expedientes.UseCases;

public class ObtenerExpedientePorIdUseCase(
    IExpedienteRepository repository,
    ITramiteRepository tramiteRepository
    )
{
    public ObtenerExpedientePorIdResponse Ejecutar(ObtenerExpedientePorIdRequest request)
    {
        Expediente expediente = repository.ObtenerExpPorId(request.expId) ?? throw new EntidadNoEncontradaException("Expediente");

        ExpedienteDTO expDto = new ExpedienteDTO(expediente.Id, expediente.Caratula.Contenido, expediente.Estado, expediente.FechaCreacion, expediente.FechaUltimaModificacion);

        IEnumerable<TramiteDTO> tramitesDto = tramiteRepository.ObtenerTramitesPorExpedienteId(request.expId)
            .Select(t => new TramiteDTO(t.Id, t.Etiqueta, t.Contenido.Texto, t.FechaCreacion, t.FechaUltimaModificacion));

        return new ObtenerExpedientePorIdResponse(expDto, tramitesDto);
    }
}
