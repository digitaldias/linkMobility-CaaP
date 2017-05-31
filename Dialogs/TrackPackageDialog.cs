using Link.Domain.Contracts;
using Link.Domain.Entities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    internal class TrackPackageDialog : IDialog<object>
    {
        private string _packageId;

        public async Task StartAsync(IDialogContext context)
        {
            await Task.FromResult<object>(null);
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var packageId = context.ExtractPackageId();

            if (string.IsNullOrEmpty(packageId))
            {
                await DisplayTrackingIdHelp(context);
                PromptDialog.Text(context, AfterPackageIdRequestAsync, "Please enter your tracking number", "Sorry, try again", 3);
            }
        }

        private async Task AfterPackageIdRequestAsync(IDialogContext context, IAwaitable<string> result)
        {
            var validator = WebApiApplication.IoCResolver.GetInstance<IPackageValidator>();
            var packageId = await result;

            if (packageId.ToLower().Trim().Contains("cancel"))
            {
                context.Done<object>(null);
                return;
            }

            if (!validator.IsValidId(packageId))
            {
                PromptDialog.Text(context, AfterPackageIdRequestAsync, "I'm so sorry, but that didn't look like a valid tracking number. Please try again, or type 'Cancel' to go back:");
            }
            else
            {
                _packageId = packageId;
                context.ConversationData.SetValue("PackageId", packageId);
                await DisplayTrackingInfoAsync(context);
                context.Done<object>(null);
            }
        }

        private async Task DisplayTrackingInfoAsync(IDialogContext context)
        {
            var manager = WebApiApplication
                .IoCResolver
                .GetInstance<IPackageManager>();

            var package = await manager.RetrievePackageInfoAsync(_packageId);

            await DisplayPackage(context, package);
        }

        private async Task DisplayPackage(IDialogContext context, Package package)
        {
            var heroCard = new HeroCard(
                "I found your package!", 
                $"Status: {package.Status}",
                $"Shipped: {package.ShipmentDate.ToShortDateString()}<br />Expected delivery: {package.ExpectedDeliveryDate.ToShortDateString()}"
                );

            var message = context.MakeMessage();
            message.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(message);
        }

        private async Task DisplayTrackingIdHelp(IDialogContext context)
        {
            var card = new HeroCard("I neeed your input",
                "In order to figure out where your package is, I'll need  you to input the tracking number for me",
                "The tracking number is either a 10-digit number or a 16 digit number (NEEDS REFERENCE)");

            var message = context.MakeMessage();
            message.Attachments.Add(card.ToAttachment());
            await context.PostAsync(message);
        }
    }
}