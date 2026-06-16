namespace SGE.Aplicacion.Autorizacion;

public class EntidadNoEncontradaException : AplicacionException
{
    public EntidadNoEncontradaException(string entidad) : base($"{entidad} no encontrado/a.") { }
}
