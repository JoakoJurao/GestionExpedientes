using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOS;

public record class ModificarPermisosUsuarioRequest(Guid UsuarioIdAModificar, List<PermisoEnum> Permisos);
