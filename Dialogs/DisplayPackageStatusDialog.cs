using Link.Domain.Contracts;
using Link.Domain.Entities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
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
                var cardImage = new CardImage("http://www.nyemotorsports.com/storage/DHL_Front_reverse.png", "Image of parcel");                
                var heroCard  = new HeroCard(
                    $"Status: {package.Status}",
                    "",
                    $"Expected delivery: {package.ExpectedDeliveryDate.ToShortDateString()}",
                    new[] { cardImage}                
                );                
                var message = context.MakeMessage();

                message.Attachments.Add(heroCard.ToAttachment());                
                await context.PostAsync(message);
                
                PromptDialog.Confirm(context, AfterAskToShowMoreAsync, "Would you like to see more information about this package?", "A regular 'yes' or 'no' will do", 3);
            }
        }

        private ReceiptCard  CreateReceiptCard(Package package)
        {
            var facts = new List<Fact> {
                new Fact("Weight", package.Weight + package.WeightUnit),
                new Fact("Width",  package.Dimensions.Width + package.Dimensions.Unit),
                new Fact("Height", package.Dimensions.Height + package.Dimensions.Unit),
                new Fact("Length", package.Dimensions.Length + package.Dimensions.Unit),
                new Fact("Delivery Address", package.DeliveryAddress.StreetAddress),
                new Fact("", package.DeliveryAddress.ZipCode),
                new Fact("", package.DeliveryAddress.City),
                new Fact("", package.DeliveryAddress.Country),
            };

            var cardImage = new CardImage("https://www.movematcher.com/wp-content/uploads/2017/03/bubbleman-courier1-974x1451.png?x59881", "In transit");

            var receiptItems = new List<ReceiptItem> {
                new ReceiptItem("Id",            null, null, cardImage, package.Id),
                new ReceiptItem("Shipment Date", null, null, null, $"{package.ShipmentDate.ToString("ddd, dd MMM yyyy")}" ),
                new ReceiptItem("Delivery Date", null, null, null, $"{package.ExpectedDeliveryDate.ToString("ddd, dd MMM yyyy")}"),
                new ReceiptItem("Delivery Time", null, null, null, package.ExpectedDeliveryDate.ToString("HH:mm"))
            };

            return new ReceiptCard("Parcel Details", receiptItems, facts);                
        }

        private async Task AfterAskToShowMoreAsync(IDialogContext context, IAwaitable<bool> result)
        {
            var wantsToSeeMore = await result;

            if (wantsToSeeMore)
            {
                var package     = context.ConversationData.GetValue<Package>("CurrentPackage");
                var receiptCard = CreateReceiptCard(package);
                var message     = context.MakeMessage();

                message.Attachments.Add(receiptCard.ToAttachment());
                await context.PostAsync(message);
            }
            context.Done<object>(null);
        }
    }
}