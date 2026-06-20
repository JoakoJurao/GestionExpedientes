using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramiteEnum Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; } = null!;
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }

    public Tramite(Guid expId, EtiquetaTramiteEnum etiqueta, ContenidoTramite contenido, Guid usuario, DateTime ahora)
    {
        Id = Guid.NewGuid();
        if (expId == Guid.Empty) throw new DominioException("El id del expediente es invalido.");
        if (usuario == Guid.Empty) throw new DominioException("El id del usuario es invalido.");
        if (!Enum.IsDefined(typeof(EtiquetaTramiteEnum), etiqueta)) throw new DominioException("La etiqueta no es valida.");
        ExpedienteId = expId;
        Etiqueta = etiqueta;
        Contenido = contenido ?? throw new DominioException("El contenido es nulo.");
        FechaCreacion = ahora;
        FechaUltimaModificacion = ahora;
        UsuarioUltimoCambio = usuario;
    }
    private void RegistrarModificacion(Guid idUsuario, DateTime ahora)
    {
        if (idUsuario == Guid.Empty) throw new DominioException("El id del usuario es invalido.");
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = ahora;
    }
    public void ModificarEtiqueta(EtiquetaTramiteEnum nuevaEtiqueta, Guid usuarioId, DateTime ahora)
    {
        if (!Enum.IsDefined(typeof(EtiquetaTramiteEnum), nuevaEtiqueta)) throw new DominioException("La etiqueta no es valida.");
        RegistrarModificacion(usuarioId, ahora);
        Etiqueta = nuevaEtiqueta;
    }
    public void ModificarContenido(ContenidoTramite nuevoContenido, Guid usuarioId, DateTime ahora)
    {
        Contenido = nuevoContenido ?? throw new DominioException("El contenido es nulo.");
        RegistrarModificacion(usuarioId, ahora);
    }
    protected Tramite() { }
}