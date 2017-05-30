using System;
using System.Threading.Tasks;

namespace Link.Domain.Contracts
{
    public interface IExceptionHandler
    {
        Task RunAsync(Func<Task> unsafeAction);


        Task<TResult> GetAsync<TResult>(Func<Task<TResult>> unsafeFunction);
    }
}
