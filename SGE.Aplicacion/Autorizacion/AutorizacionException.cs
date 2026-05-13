namespace SGE.Aplicacion.Autorizacion;
public class AutorizacionException : Exception
{
    public AutorizacionException(){}

    public AutorizacionException(string? detalle) : base(detalle){}

    public AutorizacionException(string? detalle, Exception? excepcion) : base(detalle, excepcion){}
}