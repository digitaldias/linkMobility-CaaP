using Link.Domain.Contracts;
using System;
using System.Diagnostics;

namespace Link.Data.File
{
    public class Logger : ILogger
    {
        public void LogException(Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }
}
