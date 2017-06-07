using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LogisticBot
{
    public class MyLogger : IActivityLogger
    {
        public async Task LogAsync(IActivity activity)
        {
            var activityText = JsonConvert.SerializeObject(activity);
            
            Trace.WriteLine(activityText);

            // TODO: Save log in Azure Storage. Connect IActivityLogger to the bot framework's IoC container.

            await Task.CompletedTask;
        }
    }
}