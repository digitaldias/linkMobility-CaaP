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
        private string _userName;
        

        public async Task StartAsync(IDialogContext context)
        {
            await Task.CompletedTask;
            context.Wait(MessageReceivedAsync);            
        }


        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (string.IsNullOrEmpty(_userName))
            {
                context.Call<string>(new GetName(), AfterNameDialogAsync);
            }
            else
            {                
                context.Wait(MessageReceivedAsync);
            }            
        }


        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            var images  = new[] { new CardImage("https://cldup.com/q5Jmvu10tV.png", "A bot icon") };
            var card    = new HeroCard($"Hello {_userName}!", 
                "I am the DHL Bot!", 
                "I can help you track your packages, reschedule a delivery, cancel a delivery, change delivery address and much much more! " + 
                "Just tell me what you want to do, and I will help you get it done. "  + 
                "To begin with, simply type what you need, and I'll try to help you", 
                images);

            var message = context.MakeMessage();
            message.Attachments.Add(card.ToAttachment());

            await context.PostAsync(message);
            context.Call(new LuisRoot(WebApiApplication.IoCResolver.GetInstance<ISettingsReader>()), AfterLuisRootDialog);
        }


        private async Task AfterNameDialogAsync(IDialogContext context, IAwaitable<string> result)
        {
            _userName = await result;
            context.SetUserName(_userName);

            await SendWelcomeMessageAsync(context);
        }


        private async Task AfterLuisRootDialog(IDialogContext context, IAwaitable<object> result)
        {
            await Task.CompletedTask;
            context.Wait(MessageReceivedAsync);
        }
    }
}