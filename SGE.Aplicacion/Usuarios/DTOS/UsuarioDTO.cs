using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOS;

public record class UsuarioDTO(Guid Id, string Nombre, string CorreoElectronico, bool EsAdministrador, List<PermisoEnum> Permisos);
