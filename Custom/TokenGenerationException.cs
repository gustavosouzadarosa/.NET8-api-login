namespace ApiLogin.Custom
{
    public class TokenGenerationException : Exception
    {
        public TokenGenerationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
