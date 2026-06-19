using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public record class ContenidoTramite
{
    public string Texto {get;} = "";

    public ContenidoTramite(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) throw new DominioException("Contenido en blanco.");
        Texto = texto;
    }

    protected ContenidoTramite() { }
}