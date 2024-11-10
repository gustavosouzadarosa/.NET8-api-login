namespace api_login.CustomExceptions
{
    public class UnAuthenticatedAccessException : SystemException
    {
        public UnAuthenticatedAccessException()
        { 
        }

        public UnAuthenticatedAccessException(string message) : base(message)
        {
        }

        public UnAuthenticatedAccessException(string message, Exception innerException) : base(message, innerException)

        {
        }

    }
}
