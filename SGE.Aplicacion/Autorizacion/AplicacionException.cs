namespace SGE.Aplicacion.Autorizacion;
public class AplicacionException : Exception
{
    public AplicacionException(){}

    public AplicacionException(string? detalle) : base(detalle){}

    public AplicacionException(string? detalle, Exception? excepcion) : base(detalle, excepcion){}
}