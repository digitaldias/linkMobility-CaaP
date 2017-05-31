using Link.Domain.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    internal class TrackPackageDialog : IDialog<object>
    {
        private string _packageId;

        public async Task StartAsync(IDialogContext context)
        {
            var packageId = context.ExtractPackageId();

            if (string.IsNullOrEmpty(packageId))
            {
                await DisplayTrackingIdHelp(context);
                PromptDialog.Text(context, AfterPackageIdRequestAsync, "Please enter your tracking number");
            }
        }

        private async Task AfterPackageIdRequestAsync(IDialogContext context, IAwaitable<string> result)
        {
            var validator = WebApiApplication.IoCResolver.GetInstance<IPackageValidator>();
            var packageId = await result;

            if(!validator.IsValidId(packageId))
            {
                await context.PostAsync("Yeah, uh, that didn't quite look like something I can use...");
                

            }
        }

        private async Task DisplayTrackingIdHelp(IDialogContext context)
        {
            var images = new List<CardImage> {
                new CardImage("http://webkul.com/blog/wp-content/uploads/2015/10/edited-label11.png", "Sample Tracking number Image")
            };

            var card = new HeroCard("I need your Tracking number",
                "In order to figure out where your package is, I'll need  you to input the tracking number for me",
                "The tracking number is either a 10-digit number or a 16 digit number (NEEDS REFERENCE)", images);

            var message = context.MakeMessage();
            message.Attachments.Add(card.ToAttachment());
            await context.PostAsync(message);
        }
    }
}