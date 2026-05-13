using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id {get;}
    public Guid ExpedienteId {get; private set;}
    public EtiquetaTramiteEnum Etiqueta {get; private set;}
    public ContenidoTramite Contenido {get; private set;}
    public  DateTime FechaCreacion{get;}
    public  DateTime FechaUltimaModificacion{get; private set;}
    public  Guid UsuarioUltimoCambio {get; private set;}

    public static Tramite CrearTramite(Guid expId, EtiquetaTramiteEnum etiqueta, ContenidoTramite contenido, Guid usuario, DateTime ahora)
    {
        return new Tramite(Guid.NewGuid(), expId, etiqueta, contenido, ahora, ahora, ahora, usuario);
    }

    public static Tramite ReconstruirTramite(Guid id, Guid exp_id, EtiquetaTramiteEnum etiqueta, ContenidoTramite contenido, DateTime creacion, DateTime modificacion, DateTime ahora, Guid ult_usuario)
    {
        return new Tramite(id,exp_id,etiqueta,contenido,creacion,modificacion,ahora,ult_usuario);
    }
    private void RegistrarModificacion(Guid idUsuario, DateTime ahora)
    {
        if (idUsuario == Guid.Empty) throw new DominioException("El id del usuario es invalido.");
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = ahora;
    }
    private Tramite (Guid id, Guid exp_id, EtiquetaTramiteEnum etiqueta, ContenidoTramite contenido, DateTime creacion, DateTime modificacion, DateTime ahora, Guid ult_usuario)
    {
        if (id == Guid.Empty) throw new DominioException("El id es invalido.");
        if (exp_id == Guid.Empty) throw new DominioException("El id del expediente es invalido.");
        if (!Enum.IsDefined(typeof(EtiquetaTramiteEnum), etiqueta)) throw new DominioException("La etiqueta no es valida.");
        if (creacion == DateTime.MinValue || modificacion == DateTime.MinValue) throw new DominioException("Fecha no definida");
        if (modificacion > ahora || creacion > ahora) throw new DominioException("Las fechas no pueden ser futuras.");
        if (modificacion < creacion) throw new DominioException("La fecha de ultima modificacion no puede ser menor a la fecha de creacion.");
        Id = id;
        ExpedienteId = exp_id;
        Etiqueta = etiqueta;
        Contenido = contenido ?? throw new DominioException("El contenido es nulo.");
        FechaCreacion = creacion;
        RegistrarModificacion(ult_usuario,ahora);
    }

    public void ModificarEtiqueta (EtiquetaTramiteEnum nuevaEtiqueta, Guid usuarioId, DateTime ahora)
    {
        if (!Enum.IsDefined(typeof(EtiquetaTramiteEnum), nuevaEtiqueta)) throw new DominioException("La etiqueta no es valida.");
        RegistrarModificacion(usuarioId,ahora);
        Etiqueta = nuevaEtiqueta;
    }
    public void ModificarContenido(ContenidoTramite nuevoContenido, Guid usuarioId, DateTime ahora)
    {
        Contenido = nuevoContenido ?? throw new DominioException("El contenido es nulo.");
        RegistrarModificacion(usuarioId,ahora);
    }
}