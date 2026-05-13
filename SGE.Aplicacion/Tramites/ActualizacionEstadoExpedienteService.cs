using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class ActualizacionEstadoExpedienteService(IExpedienteRepository repository_exp, ITramiteRepository repository_tramite, ITimeProvider timeProvider)
{
    public void Ejecutar(Guid expId, Guid usuarioId)
    {   
        Expediente? exp = repository_exp.ObtenerExpPorId(expId);
        if (exp is null) return;
        Tramite? ultimo = repository_tramite.ObtenerTramitesPorExpedienteId(expId).LastOrDefault(); //para no iterar a mano (hace lo mismo)
        EtiquetaTramiteEnum? ultimaEtiqueta = ultimo?.Etiqueta;
        exp.ActualizarEstado(ultimaEtiqueta, usuarioId, timeProvider.Now);
        repository_exp.EliminarExpediente(expId);
        repository_exp.AgregarExpediente(exp);
    }
}