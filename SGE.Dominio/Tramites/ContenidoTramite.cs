using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public record class ContenidoTramite
{
    public string Texto {get;}

    public ContenidoTramite(string texto)
    {
        if (texto.IsWhiteSpace()) throw new DominioException("Contenido en blanco.");
        Texto = texto;
    }
}