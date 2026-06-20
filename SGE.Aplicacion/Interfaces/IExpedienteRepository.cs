using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Interfaces;

public interface IExpedienteRepository
{
    void AgregarExpediente(Expediente exp);
    void ModificarExpediente(Expediente exp);
    Expediente? ObtenerExpPorId(Guid id);
    void EliminarExpediente(Guid id);
    IEnumerable<Expediente> ObtenerTodosExp();
}