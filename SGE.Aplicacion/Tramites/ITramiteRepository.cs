using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public interface ITramiteRepository
{
    void AgregarTramite(Tramite nuevoTramite);
    Tramite? ObtenerTramitePorId(Guid id);
    void EliminarTramite(Guid id);
    IEnumerable<Tramite> ObtenerTramitesPorExpedienteId(Guid expId); 
}