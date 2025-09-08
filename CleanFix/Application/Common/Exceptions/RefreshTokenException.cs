namespace Application.Common.Exceptions
{
    public class RefreshTokenException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public RefreshTokenException(IEnumerable<string> errors)
            : base("Refresh token failed: " + string.Join(", ", errors))
        {
            Errors = errors;
        }
    }
}
