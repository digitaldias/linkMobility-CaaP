using Link.Domain.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

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


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            if (string.IsNullOrEmpty(username))
            {
                PromptDialog.Text(context, AfterUsernamePromptAsync, "Before we get started, what is your name?");
            }
            else
            {
                var activity = await result;
                var settings = WebApiApplication.IoCResolver.GetInstance<ISettingsReader>();

                await context.Forward<object>(new LuisRootDialog(settings), AfterLuisRootDialog, activity);
            }
        }

        private async Task AfterUsernamePromptAsync(IDialogContext context, IAwaitable<string> result)
        {
            username = await result;

            context.UserData.SetValue<string>("UserName", username);

            var images = new[] { new CardImage("http://www.benniebos.com/hotair/mei2007/DHL.jpg", "A DHL Balloon") };
            var card = new HeroCard($"Hello, {username}", "Welcome to the DHL Bot!", "I can help you track your packages, reschedule a delivery, cancel a delivery, change delivery address and much much more! Just tell me what you want to do, and I will help you get it done.", images);

            var reply = context.MakeMessage();
            reply.Attachments.Add(card.ToAttachment());
            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }


        private async Task AfterLuisRootDialog(IDialogContext context, IAwaitable<object> result)
        {
            await Task.CompletedTask;
            context.Wait(MessageReceivedAsync);
        }


        
    }
}