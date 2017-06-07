using Link.Domain.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class AskForPackageId : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            Debug.WriteLine("AskForPackageId.StartAsync()");
            await context.PostAsync("Type in the package id:");
            context.Wait(MessageReceivedAsync);
        }


        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var candidate = (await result).Text;
            var validator = WebApiApplication.IoCResolver.GetInstance<IPackageValidator>();

            if(validator.IsValidId(candidate))
            {
                context.SetPackageId(candidate);
                context.Done(candidate);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}