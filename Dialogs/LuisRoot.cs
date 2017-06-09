using Link.Domain.Contracts;
using LogisticBot.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class LuisRoot : LuisDialog<object>
    {
        public LuisRoot(ISettingsReader settings)
            : base(new LuisService(new LuisModelAttribute(settings["luis.modelid"], settings["luis.subscriptionkey"])))
        {            
        }

        public override Task StartAsync(IDialogContext context)
        {            
            Debug.WriteLine("LuisRoot.StartAsync()");
            return base.StartAsync(context);
        }


        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Oh, I'm sorry, but I have no idea what you just said!");

            EntityRecommendation entity;
            if(result.TryFindEntity("builtin.datetime.date", out entity))
            {
                DateTime.Parse(entity.Entity);
            }


            context.Wait(MessageReceived);
        }


        [LuisIntent("Track package")]
        public async Task TrackPackageAsync(IDialogContext context, LuisResult result)
        {
            await Task.CompletedTask;            
            context.ConversationData.SetValue("LuisResult", result);
            context.Call<string>(new GetPackageId(), AfterPackageIdForTrackingStatus);            
        }


        [LuisIntent("ChangeAddress")]
        public async Task ChangeAddress(IDialogContext context, LuisResult result)
        {
            await Task.CompletedTask;
            context.ConversationData.SetValue("LuisResult", result);
            context.Call<string>(new GetPackageId(), AfterPackageIdForAddressChange);            
        }


        private async Task AfterPackageIdForAddressChange(IDialogContext context, IAwaitable<string> result)
        {
            var packageId = await result;

            if (string.IsNullOrEmpty(packageId))
            {
                await context.PostAsync("Ok, no package Id, so I can't change the address");
                context.Wait(MessageReceived);
            }
            else
            {
                var formDialog = FormDialog.FromForm(DeliveryAddress.BuildForm, FormOptions.PromptInStart);
                context.Call<DeliveryAddress>(formDialog, SetNewDeliveryAddressAsync);
            }
        }


        private async Task SetNewDeliveryAddressAsync(IDialogContext context, IAwaitable<DeliveryAddress> result)
        {
            var packageId          = context.FindPackageId();
            var newDeliveryAddress = await result;
            var packageManager     = WebApiApplication.IoCResolver.GetInstance<IPackageManager>();

            await packageManager.SetDeliveryAddressAsync(packageId, newDeliveryAddress);
            context.Wait(MessageReceived);
        }


        private async Task AfterPackageIdForTrackingStatus(IDialogContext context, IAwaitable<string> result)
        {
            var packageId = await result;

            if (!string.IsNullOrEmpty(packageId))
            {
                context.Call<object>(new DisplayPackageStatus(packageId), AfterDisplayPackageStatus);
            }
            else
            {
                await context.PostAsync("I'm sorry, but I cannot help you without a valid tracking Id.");
                context.Wait(MessageReceived);
            }            
        }


        private async Task AfterDisplayPackageStatus(IDialogContext context, IAwaitable<object> result)
        {
            var notUsed = await result as Activity;
            await context.PostAsync("Is there anything else I can help you with?");
            
            context.Wait(MessageReceived);
        }
    }
}