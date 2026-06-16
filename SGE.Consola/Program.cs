using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Expedientes.DTOS;
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Tramites.DTOS;
using SGE.Aplicacion.Tramites.UseCases;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;
using SGE.Infraestructura;

// Composition Root
ITimeProvider timeProvider = new SGE.Infraestructura.TimeProvider();
IExpedienteRepository expedienteRepository = new ExpedienteTxtRepository(timeProvider);
ITramiteRepository tramiteRepository = new TramiteTxtRepository(timeProvider);
IAutorizacionService autorizacionService = new AutorizacionProvisionalService();
Guid usuario = Guid.NewGuid();

var agregarExpediente = new AgregarExpedienteUseCase(autorizacionService, expedienteRepository, timeProvider);
var obtenerTodosExp   = new ObtenerTodosExpUseCase(expedienteRepository);
var agregarTramite    = new AgregarTramiteUseCase(autorizacionService, tramiteRepository, timeProvider, expedienteRepository);
var obtenerExpPorId   = new ObtenerExpPorIdUseCase(expedienteRepository);


// --- Camino Feliz ---
Console.WriteLine("--- Crear Expediente ---");

Guid idExp = Guid.Empty;
try
{
    //comentar una de las creaciones y ejecutar Program nuevamente para verificar persistencia de datos
    var nuevo1 = agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente numero 1", usuario));
    idExp = nuevo1.expId;
    Console.WriteLine("Expediente creado con id: " + idExp);
    var nuevo2 = agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente numero 2", usuario));
    idExp = nuevo2.expId;
    Console.WriteLine("Expediente creado con id: " + idExp);
}
catch (AutorizacionException ex) { Console.WriteLine("Sin permiso: " + ex.Message); }
catch (DominioException ex) { Console.WriteLine("Error de dominio: " + ex.Message); }
catch (Exception ex) { Console.WriteLine("Error inesperado: " + ex.Message); }

Console.WriteLine("Listando expedientes:");
try
{
    foreach (var exp_n in obtenerTodosExp.Ejecutar().Expedientes)
        Console.WriteLine("  " + exp_n.Id + " - " + exp_n.Caratula + " - Estado: " + exp_n.Estado);
}
catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }

Console.WriteLine("Agregando tramite PaseAEstudio...");
try
{
    var res = agregarTramite.Ejecutar(new AgregarTramiteRequest(idExp, EtiquetaTramiteEnum.PaseAEstudio, "Pase a estudio del legajo", usuario));
    Console.WriteLine("Tramite agregado con id: " + res.tramiteId);

    var exp = obtenerExpPorId.Ejecutar(new ObtenerExpPorIdRequest(idExp)).exp;
    Console.WriteLine("Estado del expediente ahora: " + exp?.Estado);
}
catch (AutorizacionException ex) { Console.WriteLine("Sin permiso: " + ex.Message); }
catch (DominioException ex) { Console.WriteLine("Error de dominio: " + ex.Message); }
catch (Exception ex) { Console.WriteLine("Error inesperado: " + ex.Message); }


// --- Caminos de Error ---
Console.WriteLine("\n--- Caminos de Error ---");

Console.WriteLine("Expediente con caratula vacia:");
try
{
    agregarExpediente.Ejecutar(new AgregarExpedienteRequest("", usuario));
}
catch (AutorizacionException ex) { Console.WriteLine("Sin permiso: " + ex.Message); }
catch (DominioException ex) { Console.WriteLine("Error de dominio: " + ex.Message); }
catch (Exception ex) { Console.WriteLine("Error inesperado: " + ex.Message); }

Console.WriteLine("Expediente sin autorizacion:");
var sinPermiso = new AgregarExpedienteUseCase(new AutorizacionDenegadaService(), expedienteRepository, timeProvider);
try
{
    sinPermiso.Ejecutar(new AgregarExpedienteRequest("Expediente valido", usuario));
}
catch (AutorizacionException ex) { Console.WriteLine("Sin permiso: " + ex.Message); }
catch (DominioException ex) { Console.WriteLine("Error de dominio: " + ex.Message); }
catch (Exception ex) { Console.WriteLine("Error inesperado: " + ex.Message); }


class AutorizacionDenegadaService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid usuarioId, PermisoEnum permisoRequerido) => false;
}
