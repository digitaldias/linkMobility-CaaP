using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class GetName : IDialog<string>
    {
        private int _attempts;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("To begin with, what should I call you? (e.g. 'Robert', 'Anne')");
            context.Wait(MessageReceivedAsync);
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text != null && message.Text.Trim().Length > 0)
            {
                context.Done(message.Text);
            }
            else
            {
                if (_attempts++ <= 2)
                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. What is your name? (e.g. 'Bill', 'Melinda')?");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("I give up. Message was not a string, or was just empty"));
                }
            }
        }
    }
}