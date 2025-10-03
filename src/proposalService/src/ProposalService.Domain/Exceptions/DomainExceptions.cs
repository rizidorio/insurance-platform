namespace ProposalService.Domain.Exceptions;

public sealed class DomainException : Exception
{
    public DomainException(string message, params object[] args) : base(string.Format(message, args))
    {
    }
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
