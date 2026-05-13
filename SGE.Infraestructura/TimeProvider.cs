using SGE.Dominio.Comun;

namespace SGE.Infraestructura;
public class TimeProvider : ITimeProvider
{
    public DateTime Now
    {
        get => DateTime.Now;
    }
}