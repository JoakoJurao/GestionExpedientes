using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;
public class Expediente
{
    public  Guid Id {get;}
    public  CaratulaExp Caratula {get; private set;}
    public  DateTime FechaCreacion{get;}
    public  DateTime FechaUltimaModificacion{get; private set;}
    public  Guid UsuarioUltimoCambio {get; private set;}
    public  EstadoExpEnum Estado{get; private set;}

    public static Expediente CrearExpediente (CaratulaExp caratula, Guid usuario, DateTime ahora) 
    {
        return new Expediente(Guid.NewGuid(), caratula, ahora, ahora, ahora, usuario, EstadoExpEnum.RecienIniciado);
    }

    public static Expediente ReconstruirExpediente(Guid id, CaratulaExp caratula, DateTime creacion, DateTime modificacion, DateTime ahora, Guid ultUsuario, EstadoExpEnum estado)
    {
        return new Expediente(id, caratula, creacion, modificacion, ahora, ultUsuario, estado);
    }

    private Expediente (Guid id, CaratulaExp caratula, DateTime creacion, DateTime modificacion, DateTime ahora, Guid ultUsuario, EstadoExpEnum estado) //  TODO: verificar si todos los parametros deberian recibirse como value objects
    {
        if (id == Guid.Empty) throw new DominioException("El id no puede estar vacio.");
        if (ultUsuario == Guid.Empty) throw new DominioException("El id del usuario es invalido.");
        if (creacion == DateTime.MinValue || modificacion == DateTime.MinValue) throw new DominioException("Fecha no definida");
        if (modificacion > ahora || creacion > ahora) throw new DominioException("Las fechas no pueden ser futuras.");
        if (modificacion < creacion) throw new DominioException("La fecha de ultima modificacion no puede ser menor a la fecha de creacion.");
        if (!Enum.IsDefined(typeof(EstadoExpEnum), estado)) throw new DominioException("El estado no es valido.");
        Id = id;
        Caratula = caratula ?? throw new DominioException("La caratula no puede ser nula.");
        UsuarioUltimoCambio = ultUsuario;
        FechaCreacion = creacion;
        FechaUltimaModificacion = modificacion;
        Estado = estado;
    }

    private void RegistrarModificacion(Guid idUsuario, DateTime ahora)
    {
        if (idUsuario == Guid.Empty) throw new DominioException("El id del usuario es invalido.");
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = ahora;
    }
    public void ModificarCaratula(CaratulaExp nuevaCaratula, Guid idUsuario, DateTime ahora)
    {
        Caratula = nuevaCaratula ?? throw new DominioException("La caratula no puede ser nula.");
        RegistrarModificacion(idUsuario, ahora);
    }

    public bool ActualizarEstado (Tramites.EtiquetaTramiteEnum? ultimaEtiqueta, Guid idUsuario, DateTime ahora)
    {   
        EstadoExpEnum EstadoActual = Estado;
        if (ultimaEtiqueta == null) Estado = EstadoExpEnum.RecienIniciado;
        else if (ultimaEtiqueta == Tramites.EtiquetaTramiteEnum.Resolucion) Estado = EstadoExpEnum.ConResolucion;
        else if (ultimaEtiqueta == Tramites.EtiquetaTramiteEnum.PaseAEstudio) Estado = EstadoExpEnum.ParaResolver;
        else if (ultimaEtiqueta == Tramites.EtiquetaTramiteEnum.PaseAlArchivo) Estado = EstadoExpEnum.Finalizado;

        RegistrarModificacion(idUsuario, ahora);
        return Estado != EstadoActual;
    }

    public void CambiarEstado(EstadoExpEnum nuevoEstado, Guid idUsuario, DateTime ahora)
    {
        if (!Enum.IsDefined(typeof(EstadoExpEnum), nuevoEstado)) throw new DominioException("El estado no es valido.");
        Estado = nuevoEstado;
        RegistrarModificacion(idUsuario, ahora);
    } 
}