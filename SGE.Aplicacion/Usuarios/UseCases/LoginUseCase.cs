using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios.DTOS;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class LoginUseCase(
    IUsuarioRepository repo,
    IHashService hashService,
    ITokenProvider tokenProvider
    )
{
    public LoginResponse Ejecutar(LoginRequest request)
    {
        Usuario? usuario = repo.ObtenerPorCorreo(request.CorreoElectronico);

        if (usuario is null || usuario.ContrasenaHash != hashService.Hash(request.Contrasena))
            throw new AutorizacionException("Correo electrónico o contraseña incorrectos.");

        string token = tokenProvider.GenerarToken(usuario);
        return new LoginResponse(token);
    }
}
