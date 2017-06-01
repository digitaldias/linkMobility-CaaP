using Link.Domain.Contracts;
using Link.Domain.Entities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class DisplayPackageStatusDialog : IDialog<object>
    {
        private string _packageId;


        public DisplayPackageStatusDialog(string packageId)
        {
            _packageId = packageId;
        }


        public async Task StartAsync(IDialogContext context)
        {
            await DisplayPackageStatusAsync(context);
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await Task.CompletedTask;
        }


        private async Task DisplayPackageStatusAsync(IDialogContext context)
        {
            await context.PostAsync("Looking for your package... be right back!");

            var manager = WebApiApplication.IoCResolver.GetInstance<IPackageManager>();
            var package = await manager.RetrievePackageInfoAsync(_packageId);

            if (package == null)
            {
                await context.PostAsync($"I'm sorry, but I couldn't find a package with tracking number '{_packageId}'. ");
                context.Done<object>(null);
            }
            else
            {
                context.ConversationData.SetValue<Package>("CurrentPackage", package);
                var heroCard = new HeroCard(
                    "I found your package!",
                    $"Status: {package.Status}",
                    $"Expected delivery: {package.ExpectedDeliveryDate.ToShortDateString()}"
                );
                var message = context.MakeMessage();
                message.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(message);
                
                PromptDialog.Confirm(context, AfterAskToShowMoreAsync, "Would you like to see more information about this package?", "A regular 'yes' or 'no' will do", 3);
            }
        }


        private async Task AfterAskToShowMoreAsync(IDialogContext context, IAwaitable<bool> result)
        {
            var wantsToSeeMore = await result;

            if (wantsToSeeMore)
            {
                //TODO: Make pretty card-based message
                var package = context.ConversationData.GetValue<Package>("CurrentPackage");
                await context.PostAsync($"Here are some more details: Weight {package.Weight}{package.WeightUnit}");
            }
            context.Done<object>(null);
        }
    }
}