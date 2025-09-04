namespace Application.Common.Exceptions;

public class ExternalServiceException<T> : Exception
{
    public T Id { get; }
    public string[] ServiceErrors { get; }

    public ExternalServiceException(T id, IEnumerable<string> errors) 
        : base($"External service call failed for BuildingCode '{id}': {string.Join(", ", errors)}")
    {
        Id = id;
        ServiceErrors = errors.ToArray();
    }

    public ExternalServiceException(T id, string error) 
        : this(id, new[] { error })
    {
    }
}
