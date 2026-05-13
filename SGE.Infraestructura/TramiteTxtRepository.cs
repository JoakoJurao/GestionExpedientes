using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Infraestructura;

public class TramiteTxtRepository(ITimeProvider timeProvider) : ITramiteRepository
{
    string _nombreArchivo = "Tramites.txt";
    public void AgregarTramite(Tramite nuevoTramite)
    {
        using StreamWriter writer = new StreamWriter(_nombreArchivo,true);
        writer.WriteLine(nuevoTramite.Id);
        writer.WriteLine(nuevoTramite.ExpedienteId);
        writer.WriteLine(nuevoTramite.Etiqueta);
        writer.WriteLine(nuevoTramite.Contenido.Texto);
        writer.WriteLine(nuevoTramite.FechaCreacion);
        writer.WriteLine(nuevoTramite.FechaUltimaModificacion);
        writer.WriteLine(nuevoTramite.UsuarioUltimoCambio);
    }

    public Tramite? ObtenerTramitePorId(Guid idBuscado)
    {
        if (!File.Exists(_nombreArchivo)) return null;
        using StreamReader reader = new StreamReader(_nombreArchivo);
        string id = "";
        string expedienteId="";
        string etiqueta="";
        string contenido="";
        string fechaCreacion="";
        string fechaUltimaModificacion="";
        string usuarioUltimoCambio="";
        while (!reader.EndOfStream)
        {
            id = reader.ReadLine() ?? "";
            if (id == idBuscado.ToString())
            {
                expedienteId = reader.ReadLine() ?? "";
                etiqueta = reader.ReadLine() ?? "";
                contenido = reader.ReadLine() ?? "";
                fechaCreacion = reader.ReadLine() ?? "";
                fechaUltimaModificacion = reader.ReadLine() ?? "";
                usuarioUltimoCambio = reader.ReadLine() ?? "";
                break;
            } else
            {
                for (int i=0;i<6;i++) reader.ReadLine();
            }
        }
        if (id == idBuscado.ToString())
        {
            Tramite tramite = Tramite.ReconstruirTramite(
                new Guid(id),
                new Guid(expedienteId),
                Enum.Parse<EtiquetaTramiteEnum>(etiqueta),
                new ContenidoTramite(contenido),
                DateTime.Parse(fechaCreacion),
                DateTime.Parse(fechaUltimaModificacion),
                timeProvider.Now,
                new Guid(usuarioUltimoCambio)
            );
            return tramite;
        }
        return null;
    }
    public void EliminarTramite(Guid tramiteId)
    {
        if (!File.Exists(_nombreArchivo)) return;
        using StreamReader reader = new StreamReader(_nombreArchivo);
        int i=0; int limite =-1;
        List<String> contenido = new List<string>();
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine() ?? throw new RepositorioException("Archivo de tramites dañado");
            if (i > limite)
            {
            contenido.Add(line);
            if (i%7==0 && line == tramiteId.ToString())
                {
                limite = i+6;
                contenido.RemoveAt(contenido.Count - 1);
                }
            }
            i++;
        }
        using StreamWriter writer = new StreamWriter(_nombreArchivo, false); // lo sobreescribe
        foreach (String linea in contenido)
        {
            writer.WriteLine(linea);
        }
    }
    public IEnumerable<Tramite> ObtenerTramitesPorExpedienteId(Guid expId)
    {
        if (!File.Exists(_nombreArchivo)) yield break;
        using StreamReader reader = new StreamReader(_nombreArchivo);
        string id = "";
        string expedienteId="";
        string etiqueta="";
        string contenido="";
        string fechaCreacion="";
        string fechaUltimaModificacion="";
        string usuarioUltimoCambio="";
        while (!reader.EndOfStream)
        {
            id = reader.ReadLine() ?? "";
            expedienteId = reader.ReadLine() ?? "";
            if (expedienteId == expId.ToString())
            {
                etiqueta = reader.ReadLine() ?? "";
                contenido = reader.ReadLine() ?? "";
                fechaCreacion = reader.ReadLine() ?? "";
                fechaUltimaModificacion = reader.ReadLine() ?? "";
                usuarioUltimoCambio = reader.ReadLine() ?? "";

                Tramite tramite = Tramite.ReconstruirTramite(
                new Guid(id),
                new Guid(expedienteId),
                Enum.Parse<EtiquetaTramiteEnum>(etiqueta),
                new ContenidoTramite(contenido),
                DateTime.Parse(fechaCreacion),
                DateTime.Parse(fechaUltimaModificacion),
                timeProvider.Now,
                new Guid(usuarioUltimoCambio));

                yield return tramite;
            } else
            {
                for (int i=0;i<5;i++) reader.ReadLine();//
            }
        }
        yield break;
    }
}