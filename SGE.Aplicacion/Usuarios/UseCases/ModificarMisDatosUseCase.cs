using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios.DTOS;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ModificarMisDatosUseCase(
    IUsuarioRepository repository,
    IHashService hashService,
    IUnidadDeTrabajo udt
    )
{
    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest request, Guid idUsuarioAutenticado)
    {
        Usuario usuario = repository.ObtenerPorId(idUsuarioAutenticado)
            ?? throw new EntidadNoEncontradaException("Usuario");

        if (request.Nombre is not null)
            usuario.ModificarNombre(request.Nombre);

        if (request.CorreoElectronico is not null)
            usuario.ModificarCorreo(request.CorreoElectronico);

        if (request.NuevaContrasena is not null)
            usuario.ModificarContrasena(hashService.Hash(request.NuevaContrasena));

        udt.Guardar();
        return new ModificarMisDatosResponse();
    }
}
