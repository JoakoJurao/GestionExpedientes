using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class ActualizacionEstadoExpedienteService(IExpedienteRepository repository_exp, ITramiteRepository repository_tramite, ITimeProvider timeProvider)
{
    public void Ejecutar(Guid expId, Guid usuarioId)
    {
        Expediente? exp = repository_exp.ObtenerExpPorId(expId) ?? throw new EntidadNoEncontradaException("Expediente");
        Tramite? ultimo = repository_tramite.ObtenerUltimoTramiteDeExpediente(expId) ?? throw new EntidadNoEncontradaException("Tramite");
        EtiquetaTramiteEnum? ultimaEtiqueta = ultimo?.Etiqueta;
        exp.ActualizarEstado(ultimaEtiqueta, usuarioId, timeProvider.Now);
        repository_exp.EliminarExpediente(expId);
        repository_exp.AgregarExpediente(exp);
    }
}