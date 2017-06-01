using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;

namespace LogisticBot
{
    public class MyLogger : IActivityLogger
    {
        public async Task LogAsync(IActivity activity)
        {
            // TODO: Implement this later
            // Remember to register this instance in AutoFac config somewhere..:)
            await Task.CompletedTask;
        }
    }
}