using System;

namespace Link.Domain.Contracts
{
    public interface ILogger
    {
        void LogException(Exception ex);
    }
}
