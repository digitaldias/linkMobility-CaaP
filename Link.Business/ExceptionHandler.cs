using Link.Domain.Contracts;
using System;
using System.Threading.Tasks;

namespace Link.Business
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;

        public ExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }


        public async Task<TResult> GetAsync<TResult>(Func<Task<TResult>> unsafeFunction)
        {
            try
            {
                return await unsafeFunction.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            return await Task.FromResult(default(TResult));
        }


        public async Task RunAsync(Func<Task> unsafeAction)
        {
            try
            {
                await unsafeAction.Invoke();
            }
            catch (Exception ex)
            {          
                _logger.LogException(ex);
            }
        }
    }
}
