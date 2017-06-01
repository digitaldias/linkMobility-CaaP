using Link.Domain.Contracts;
using Link.Domain.Entities;
using LogisticBot.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class LuisRootDialog : LuisDialog<object>
    {
        public LuisRootDialog(ISettingsReader settings)
            : base(new LuisService(new LuisModelAttribute(settings["luis.modelid"], settings["luis.subscriptionkey"])))
        {
        }


        [LuisIntent("Track package")]
        public async Task TrackPackage(IDialogContext context, LuisResult result)
        {
            context.ConversationData.SetValue("LuisResult", result);
            var packageId = context.ExtractPackageId();
            if(string.IsNullOrEmpty(packageId))
            {
                await context.Forward<string>(new ObtainPackageIdDialog(), AfterPackageIdForTrackingStatus, context.Activity as IMessageActivity);
            }
            else
            {
                var awaitablePackageId = new AwaitableFromItem<string>(packageId);
                await AfterPackageIdForTrackingStatus(context, awaitablePackageId);
            }
        }


        [LuisIntent("ChangeAddress")]
        public async Task ChangeAddress(IDialogContext context, LuisResult result)
        {
            var packageId = context.ExtractPackageId();
           
            var dialog = FormDialog.FromForm(DeliveryAddress.BuildForm, FormOptions.PromptInStart);  
            await Task.Run(() => context.Call(dialog, AfterDeliveryAddress));
        }


        private async Task AfterDeliveryAddress(IDialogContext context, IAwaitable<DeliveryAddress> result)
        {
            var deliveryAddress = await result;
            await context.PostAsync("Ok, Let me change that address for you!");
            var packageManager = WebApiApplication.IoCResolver.GetInstance<IPackageManager>();

            // await packageManager.SetDeliveryAddressAsync(packageId, deliveryAddress);

            context.Wait(MessageReceived);
        }


        private async Task AfterPackageIdForTrackingStatus(IDialogContext context, IAwaitable<string> result)
        {
            var packageId = await result;

            if (!string.IsNullOrEmpty(packageId))
            {
                context.Call<object>(new DisplayPackageStatusDialog(packageId), AfterDisplayPackageStatus);
            }
            else
            {
                await context.PostAsync("I'm sorry, but I cannot help you without a valid tracking Id.");
                context.Wait(MessageReceived);
            }
        }


        private async Task AfterDisplayPackageStatus(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Is there anything else I can help you with?");
            
            context.Wait(MessageReceived);
        }



        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I have no idea what you just said!");

            context.Wait(MessageReceived);
        }

    }
}