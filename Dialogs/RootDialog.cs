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


        public async Task StartAsync(IDialogContext context)
        {
            await Task.CompletedTask;
            context.Wait(MessageReceivedAsync);            
        }


        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            await SendWelcomeMessageAsync(context);
        }


        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            var images  = new[] { new CardImage("https://cldup.com/q5Jmvu10tV.png", "A bot icon") };
            var card    = new HeroCard($"Hello!", "I am the DHL Bot!", "I can help you track your packages, reschedule a delivery, cancel a delivery, change delivery address and much much more! Just tell me what you want to do, and I will help you get it done.", images);
            var message = context.MakeMessage();

            message.Attachments.Add(card.ToAttachment());
            await context.PostAsync(message);

            context.Call(new GetName(), AfterNameDialogAsync);
        }


        private async Task AfterNameDialogAsync(IDialogContext context, IAwaitable<string> result)
        {
            username = await result;
            context.SetUserName(username);
            await context.PostAsync($"Allrigt, {username}, what can I help you with?");
            context.Call(new LuisRoot(WebApiApplication.IoCResolver.GetInstance<ISettingsReader>()), AfterLuisRootDialog);
        }


        private Task AfterLuisRootDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }
    }
}