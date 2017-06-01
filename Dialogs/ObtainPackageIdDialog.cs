using Link.Domain.Contracts;
using Link.Domain.Entities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    internal class ObtainPackageIdDialog : IDialog<string>
    {
        int attempts = 3;


        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Please type the tracking number:");
            context.Wait(MessageReceivedAsync);
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var candidate = (string)await result;
            var validator = WebApiApplication.IoCResolver.GetInstance<IPackageValidator>();

            if (validator.IsValidId(candidate))
            {
                context.ConversationData.SetValue("PackageId", candidate);
                context.Done(candidate);
            }
            else
            {
                if(attempts-- > 0)
                {
                    await DisplayTrackingIdHelp(context);
                    context.Wait(MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Input was either empty or not something I can see as a tracking number"));
                }
            }
                            
        }



        private async Task DisplayTrackingIdHelp(IDialogContext context)
        {
            var heroCard = new HeroCard("Tracking Number",
                "",
                "Your tracking ID is either a 12-digit number, or a 16-digit number. (MORE INFO HERE)"
                );

            var message = context.MakeMessage();
            message.Attachments.Add(heroCard.ToAttachment());

            await context.PostAsync(message);
        }
    }
}