namespace ApiLogin.Custom
{
    public class GenericApplicationException : Exception
    {
        public GenericApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
