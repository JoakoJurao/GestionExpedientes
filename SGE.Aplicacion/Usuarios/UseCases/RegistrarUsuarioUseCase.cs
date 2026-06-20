using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios.DTOS;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class RegistrarUsuarioUseCase(
    IUsuarioRepository repository,
    IHashService hashService,
    IUnidadDeTrabajo udt
    )
{
    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        if (repository.ObtenerPorCorreo(request.CorreoElectronico) is not null)
            throw new AplicacionException("El correo electrónico ya se encuentra registrado.");

        string contrasenaHash = hashService.Hash(request.Contrasena);
        Usuario usuario = new Usuario(request.Nombre, request.CorreoElectronico, contrasenaHash, false);
        repository.AgregarUsuario(usuario);
        udt.Guardar();
        return new RegistrarUsuarioResponse(usuario.Id);
    }
}
