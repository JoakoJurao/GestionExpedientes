namespace SGE.Dominio.Usuarios;

public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, PermisoEnum permisoRequerido);
}