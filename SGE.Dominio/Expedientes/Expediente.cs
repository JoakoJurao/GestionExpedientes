using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; }
    public CaratulaExp Caratula { get; private set; } = null!;
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public EstadoExpEnum Estado { get; private set; }

    public Expediente(CaratulaExp caratula, Guid usuario, DateTime ahora)
    {
        Id = Guid.NewGuid();
        if (usuario == Guid.Empty) throw new DominioException("El id del usuario es invalido.");
        Caratula = caratula ?? throw new DominioException("La caratula no puede ser nula.");
        UsuarioUltimoCambio = usuario;
        FechaCreacion = ahora;
        FechaUltimaModificacion = ahora;
        Estado = EstadoExpEnum.RecienIniciado;
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

    public bool ActualizarEstado(Tramites.EtiquetaTramiteEnum? ultimaEtiqueta, Guid idUsuario, DateTime ahora)
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
    protected Expediente() { }
}
