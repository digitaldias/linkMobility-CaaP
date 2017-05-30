using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs.Internals;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        string username;

        public Task StartAsync(IDialogContext context)
        {

            if (string.IsNullOrEmpty(username))
            {
                context.PostAsync("Welcome user! What is your name?");

            }

            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;

            if (string.IsNullOrEmpty(username))
            {
                username = activity.Text;
            }
            else
            {
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                await context.PostAsync($"You sent {activity.Text} which was {length} characters");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}