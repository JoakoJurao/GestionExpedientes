namespace SGE.Aplicacion.Autorizacion;
public class RepositorioException : Exception
{
    public RepositorioException(){}

    public RepositorioException(string? detalle) : base(detalle){}

    public RepositorioException(string? detalle, Exception? excepcion) : base(detalle, excepcion){}
}