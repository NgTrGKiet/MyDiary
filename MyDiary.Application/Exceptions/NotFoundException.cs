namespace MyDiary.Application.Exceptions;

public class NotFoundException : System.Exception
{
    public NotFoundException(string name, object key) : base($"{name} ({key}) was not found")
    {
            
    }
}