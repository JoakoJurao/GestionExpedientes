namespace SGE.Dominio.Comun;

public class DominioException : Exception
{
    public DominioException(){}

    public DominioException(string? detalle) : base(detalle){}

    public DominioException(string? detalle, Exception? excepcion) : base(detalle, excepcion){} 
}