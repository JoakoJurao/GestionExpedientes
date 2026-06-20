using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Tramites.UseCases;
using SGE.Aplicacion.Usuarios.UseCases;

namespace SGE.Aplicacion;

public static class AplicacionExtensions
{
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        services.AddScoped<AgregarExpedienteUseCase>();
        services.AddScoped<CambiarEstadoExpedienteUseCase>();
        services.AddScoped<EliminarExpedienteUseCase>();
        services.AddScoped<ModificarCaratulaExpedienteUseCase>();
        services.AddScoped<ObtenerExpedientePorIdUseCase>();
        services.AddScoped<ObtenerTodosExpUseCase>();

        services.AddScoped<AgregarTramiteUseCase>();
        services.AddScoped<EliminarTramiteUseCase>();
        services.AddScoped<ModificarContenidoTramiteUseCase>();
        services.AddScoped<ObtenerTramitePorExpedienteIdUseCase>();

        services.AddScoped<RegistrarUsuarioUseCase>();
        services.AddScoped<LoginUseCase>();
        services.AddScoped<ModificarMisDatosUseCase>();
        services.AddScoped<ListarUsuariosUseCase>();
        services.AddScoped<EliminarUsuarioUseCase>();
        services.AddScoped<ModificarPermisosUsuarioUseCase>();

        services.AddScoped<ActualizacionEstadoExpedienteService>();

        return services;
    }
}
