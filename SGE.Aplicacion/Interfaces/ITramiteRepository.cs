using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Interfaces;

public interface ITramiteRepository
{
    void AgregarTramite(Tramite nuevoTramite);
    void ModificarTramite(Tramite tramite);
    Tramite? ObtenerTramitePorId(Guid id);
    void EliminarTramite(Guid id);
    IEnumerable<Tramite> ObtenerTramitesPorExpedienteId(Guid expId);
    Tramite? ObtenerUltimoTramiteDeExpediente(Guid expId);
}