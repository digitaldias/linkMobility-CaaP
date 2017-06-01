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
        public async Task StartAsync(IDialogContext context)
        {
            await Task.FromResult<object>(null);
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var packageId = context.ExtractPackageId();

            await Task.FromResult<object>(null);

            if (string.IsNullOrEmpty(packageId))
            {
                PromptDialog.Text(context, AfterPackageIdRequestAsync, "Please enter your tracking number", "Please try again", 3);
            }
            else
            {
                context.Done<string>(packageId);
            }
        }

        private async Task AfterPackageIdRequestAsync(IDialogContext context, IAwaitable<string> result)
        {
            var validator = WebApiApplication.IoCResolver.GetInstance<IPackageValidator>();
            var packageId = await result;

            if (packageId.ToLower().Trim().Contains("cancel"))
            {
                context.Done<string>(string.Empty);
                return;
            }

            if (!validator.IsValidId(packageId))
            {
                await DisplayTrackingIdHelp(context);
                PromptDialog.Text(context, AfterPackageIdRequestAsync, "That didn't look like a valid tracking number. Please try again, or type 'Cancel' to go back:");
            }
            else
            {
                context.Done(packageId);
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