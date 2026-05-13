using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;

public record class CaratulaExp
{
    public string Contenido {get;}

    public CaratulaExp(string contenido)
    {
        if (string.IsNullOrWhiteSpace(contenido)) throw new DominioException("Caratula en blanco.");
        Contenido = contenido;
    }
}