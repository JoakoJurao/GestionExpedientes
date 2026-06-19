using SGE.Dominio.Comun;

namespace SGE.Dominio.Usuarios;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = "";
    public string CorreoElectronico { get; private set; } = null!;
    public string ContrasenaHash { get; private set; } = "";
    public bool EsAdministrador { get; private set; }

    public List<PermisoEnum> Permisos { get; private set; } = [];

    public Usuario(string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador, List<PermisoEnum>? permisos = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DominioException("El nombre del usuario no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(correoElectronico))
            throw new DominioException("El correo electrónico del usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(contrasenaHash))
            throw new DominioException("La contraseña del usuario no puede estar vacía.");

        Id = Guid.NewGuid();
        Nombre = nombre;
        CorreoElectronico = correoElectronico;
        ContrasenaHash = contrasenaHash;
        EsAdministrador = esAdministrador;
        if (permisos is not null)
            Permisos = permisos;
    }
    public void AgregarPermiso(PermisoEnum nuevoPermiso)
    {
        if (!Permisos.Contains(nuevoPermiso))
            Permisos.Add(nuevoPermiso);
    }

    public void QuitarPermiso(PermisoEnum permiso)
    {
        Permisos.Remove(permiso);
    }

    public bool TienePermiso(PermisoEnum permiso)
    {
        return EsAdministrador || Permisos.Contains(permiso);
    }

    protected Usuario() { }
}
