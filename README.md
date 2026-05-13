# Guía de prueba — SGE.Consola

## Cómo ejecutar

Desde la carpeta `SGE.Consola`:

```
dotnet run
```

No se requiere configuración previa. Los repositorios usan archivos de texto como persistencia y se crean automáticamente en la primera ejecución.

---

## Composition Root

Lo primero que hace `Program.cs` es instanciar todas las dependencias manualmente y pasárselas a los casos de uso por constructor. Esto cumple el rol de Composition Root sin ningún contenedor de inyección de dependencias.

```csharp
ITimeProvider timeProvider = new SGE.Infraestructura.TimeProvider();
IExpedienteRepository expedienteRepository = new ExpedienteTxtRepository(timeProvider);
ITramiteRepository tramiteRepository = new TramiteTxtRepository(timeProvider);
IAutorizacionService autorizacionService = new AutorizacionProvisionalService();
Guid usuario = Guid.NewGuid();

var agregarExpediente = new AgregarExpedienteUseCase(autorizacionService, expedienteRepository, timeProvider);
var obtenerTodosExp   = new ObtenerTodosExpUseCase(expedienteRepository);
var agregarTramite    = new AgregarTramiteUseCase(autorizacionService, tramiteRepository, timeProvider, expedienteRepository);
var obtenerExpPorId   = new ObtenerExpPorIdUseCase(expedienteRepository);
```

`AutorizacionProvisionalService` siempre retorna `true`, por lo que todos los permisos están habilitados durante las pruebas del camino feliz.

---

## Camino Feliz

### 1. Crear expedientes

Se crean dos expedientes mediante `AgregarExpedienteRequest`. El caso de uso valida permisos, construye el objeto de dominio `Expediente` con su `CaratulaExp`, y lo persiste.

```csharp
var nuevo1 = agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente numero 1", usuario));
idExp = nuevo1.expId;
Console.WriteLine("Expediente creado con id: " + idExp);

var nuevo2 = agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente numero 2", usuario));
idExp = nuevo2.expId;
Console.WriteLine("Expediente creado con id: " + idExp);
```

Salida:
```
--- Crear Expediente ---
Expediente creado con id: 8d5f9cf3-525e-474d-99db-fa63c11d102d
Expediente creado con id: 03883156-771d-4c2a-a097-9654e19b55f7
```

### 2. Listar expedientes

`ObtenerTodosExpUseCase` devuelve todos los expedientes persistidos. El estado inicial de todo expediente es `RecienIniciado`.

```csharp
foreach (var exp_n in obtenerTodosExp.Ejecutar().expedientes)
    Console.WriteLine("  " + exp_n.Id + " - " + exp_n.Caratula.Contenido + " - Estado: " + exp_n.Estado);
```

Salida:
```
Listando expedientes:
  8d5f9cf3-525e-474d-99db-fa63c11d102d - Expediente numero 1 - Estado: RecienIniciado
  03883156-771d-4c2a-a097-9654e19b55f7 - Expediente numero 2 - Estado: RecienIniciado
```

### 3. Agregar un trámite y verificar cambio de estado

Al agregar un trámite con etiqueta `PaseAEstudio`, el servicio `ActualizacionEstadoExpedienteService` actualiza automáticamente el estado del expediente asociado a `ParaResolver`.

```csharp
var res = agregarTramite.Ejecutar(new AgregarTramiteRequest(
    idExp,
    EtiquetaTramiteEnum.PaseAEstudio,
    "Pase a estudio del legajo",
    usuario
));
Console.WriteLine("Tramite agregado con id: " + res.tramiteId);

var exp = obtenerExpPorId.Ejecutar(new ObtenerExpPorIdRequest(idExp)).exp;
Console.WriteLine("Estado del expediente ahora: " + exp?.Estado);
```

Salida:
```
Agregando tramite PaseAEstudio...
Tramite agregado con id: 327846f1-04dc-4278-a7e9-e82b11dd705c
Estado del expediente ahora: ParaResolver
```

Tabla de transiciones de estado según la etiqueta del último trámite:

| Etiqueta del trámite | Estado resultante |
|---|---|
| (ningún trámite) | RecienIniciado |
| PaseAEstudio | ParaResolver |
| Resolucion | ConResolucion |
| PaseAlArchivo | Finalizado |

---

## Caminos de Error

Todos los bloques `try/catch` distinguen tres tipos de excepción:

- `DominioException` — la regla de negocio fue violada (datos inválidos)
- `AutorizacionException` — el usuario no tiene el permiso requerido
- `Exception` — error genérico inesperado

### Error 1: Carátula vacía

`CaratulaExp` valida en su constructor que el contenido no sea nulo ni vacío. Si lo es, lanza `DominioException` antes de que el expediente llegue al repositorio.

```csharp
try
{
    agregarExpediente.Ejecutar(new AgregarExpedienteRequest("", usuario));
}
catch (AutorizacionException ex) { Console.WriteLine("Sin permiso: " + ex.Message); }
catch (DominioException ex)      { Console.WriteLine("Error de dominio: " + ex.Message); }
catch (Exception ex)             { Console.WriteLine("Error inesperado: " + ex.Message); }
```

Salida:
```
Expediente con caratula vacia:
Error de dominio: Caratula en blanco.
```

### Error 2: Sin autorización

Se instancia un caso de uso con `AutorizacionDenegadaService`, una clase local cuyo `PoseeElPermiso` siempre retorna `false`. Esto simula el comportamiento que tendría el sistema con un usuario sin permisos, equivalente a cambiar `AutorizacionProvisionalService` para que retorne `false`.

```csharp
class AutorizacionDenegadaService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid usuarioId, PermisoEnum permisoRequerido) => false;
}

var sinPermiso = new AgregarExpedienteUseCase(new AutorizacionDenegadaService(), expedienteRepository, timeProvider);
try
{
    sinPermiso.Ejecutar(new AgregarExpedienteRequest("Expediente valido", usuario));
}
catch (AutorizacionException ex) { Console.WriteLine("Sin permiso: " + ex.Message); }
catch (DominioException ex)      { Console.WriteLine("Error de dominio: " + ex.Message); }
catch (Exception ex)             { Console.WriteLine("Error inesperado: " + ex.Message); }
```

Salida:
```
Expediente sin autorizacion:
Sin permiso: No posee los permisos necesarios
```

---

## Verificar persistencia

Los repositorios guardan los datos en archivos de texto. Para verificar que la persistencia funciona entre ejecuciones, comentar una de las creaciones y volver a correr el programa: los expedientes anteriores siguen apareciendo en el listado.

```csharp
// comentar una y ejecutar de nuevo para ver que el otro persiste
var nuevo1 = agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente numero 1", usuario));
// var nuevo2 = agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente numero 2", usuario));
```
