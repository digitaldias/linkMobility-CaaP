using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class ConfirmPackageIdResuse : IDialog<bool>
    {
        private string _packageId;


        public ConfirmPackageIdResuse(string packageId)
        {
            _packageId = packageId;
        }


        public async Task StartAsync(IDialogContext context)
        {            
            await context.PostAsync($"Shall I reuse package '{_packageId}'?");
            context.Wait(MessageReceivedAsync);
        }


        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = (await result).Text;
            var answer = message.ToLower().Trim();
            var positiveAnswers = new[] { "yes", "y", "sure", "ok", "yeah", "true", "positive", "confirm", "absolutely" };

            foreach (var choice in positiveAnswers)
            {
                if (answer.Contains(choice))
                    context.Done(true);
            }
            context.Done(false);
        }
    }
}