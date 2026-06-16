using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public interface IExpedienteRepository
{
    void AgregarExpediente(Expediente exp);
    void ModificarExpediente(Expediente exp);
    Expediente? ObtenerExpPorId(Guid id);
    void EliminarExpediente(Guid id);
    IEnumerable<Expediente> ObtenerTodosExp();
}