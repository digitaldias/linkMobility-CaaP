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

            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task OnNameReceived(IDialogContext context, IAwaitable<string> result)
        {
            username = await result;

            var images = new[] { new CardImage("https://www.dhl.de/content/dam/dhlde/images/paket-neu/2014/header-720/Paket_720x233.jpg", "A person") };
            var card = new HeroCard($"Hello, {username}", "Welcome to the DHL Bot!", "I can help you do this, and that. Here are some suggestions to get you started...", images);

            var reply = context.MakeMessage();
            reply.Attachments.Add(card.ToAttachment());
            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;

            if (string.IsNullOrEmpty(username))
            {
                PromptDialog.Text(context, OnNameReceived, "What is your name?");
            }
            else
            {
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                await context.PostAsync($"You sent {activity.Text} which was {length} characters");

                context.Wait(MessageReceivedAsync);
            }

        }
    }
}