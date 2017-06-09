using Link.Domain.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class AskForPackageId : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {            
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
                await context.PostAsync("That didn't look like a valid PackageId. Please try again");
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}