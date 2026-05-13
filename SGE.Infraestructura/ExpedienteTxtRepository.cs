using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;

namespace SGE.Infraestructura;

public class ExpedienteTxtRepository(ITimeProvider timeProvider) : IExpedienteRepository
{
    private readonly string _nombreArchivo = "expedientes.txt";
    public void AgregarExpediente(Expediente exp)
    {
        using StreamWriter writer = new StreamWriter(_nombreArchivo,true);
        writer.WriteLine(exp.Id);
        writer.WriteLine(exp.Caratula.Contenido);
        writer.WriteLine(exp.FechaCreacion);
        writer.WriteLine(exp.FechaUltimaModificacion);
        writer.WriteLine(exp.UsuarioUltimoCambio);
        writer.WriteLine(exp.Estado);
    }
    public Expediente? ObtenerExpPorId(Guid idBuscado)
    {
        if (!File.Exists(_nombreArchivo)) return null;
        using StreamReader reader = new StreamReader(_nombreArchivo);
        string id = "";
        string caratula="";
        string fechaCreacion="";
        string fechaUltimaModificacion="";
        string usuarioUltimoCambio="";
        string estado="";
        while (!reader.EndOfStream)
        {
            id = reader.ReadLine() ?? "";
            if (id == idBuscado.ToString())
            {
                caratula = reader.ReadLine() ?? "";
                fechaCreacion = reader.ReadLine() ?? "";
                fechaUltimaModificacion = reader.ReadLine() ?? "";
                usuarioUltimoCambio = reader.ReadLine() ?? "";
                estado = reader.ReadLine() ?? "";
                break;
            } else
            {
                for (int i=0;i<5;i++) reader.ReadLine();
            }
        }
        if (id == idBuscado.ToString())
        {
            Expediente exp = Expediente.ReconstruirExpediente(
                new Guid(id),
                new CaratulaExp(caratula),
                DateTime.Parse(fechaCreacion),
                DateTime.Parse(fechaUltimaModificacion),
                timeProvider.Now,
                new Guid(usuarioUltimoCambio),
                Enum.Parse<EstadoExpEnum>(estado)
            );
            return exp;
        }
        return null;
    }
    public void EliminarExpediente(Guid expId)
    {
        if (!File.Exists(_nombreArchivo)) return;
        using StreamReader reader = new StreamReader(_nombreArchivo);
        int i=0; int limite =-1;
        List<String> contenido = new List<string>();
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine() ?? throw new RepositorioException("Archivo de expedientes dañado");
            if (i > limite)
            {
            contenido.Add(line);
            if (i%6==0 && line == expId.ToString())
                {
                limite = i+5;
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
    public IEnumerable<Expediente> ObtenerTodosExp()
    {
        if (!File.Exists(_nombreArchivo)) yield break;
        using StreamReader reader = new StreamReader(_nombreArchivo);
        while (!reader.EndOfStream)
        {
            string id = reader.ReadLine() ?? "";
            string caratula = reader.ReadLine() ?? "";
            string fechaCreacion = reader.ReadLine() ?? "";
            string fechaUltimaModificacion = reader.ReadLine() ?? "";
            string usuarioUltimoCambio = reader.ReadLine() ?? "";
            string estado = reader.ReadLine() ?? "";
            Expediente exp = Expediente.ReconstruirExpediente(
                new Guid(id),
                new CaratulaExp(caratula),
                DateTime.Parse(fechaCreacion),
                DateTime.Parse(fechaUltimaModificacion),
                timeProvider.Now,
                new Guid(usuarioUltimoCambio),
                Enum.Parse<EstadoExpEnum>(estado)
            );
            yield return exp;
        }
        yield break;
    }
}