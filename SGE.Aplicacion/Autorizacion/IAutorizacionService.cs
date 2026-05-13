namespace SGE.Aplicacion.Autorizacion;

public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, PermisoEnum permisoRequerido);
}