using Link.Domain.Contracts;
using Link.Domain.Entities;
using Microsoft.Bot.Builder.Dialogs;
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
            await context.Forward<object>(new ObtainPackageIdDialog(), AfterTrackPackageDialog, context.Activity as IMessageActivity);
                
        }


        private async Task AfterTrackPackageDialog(IDialogContext context, IAwaitable<object> result)
        {
            var packageId = await result as string;
            if (!string.IsNullOrEmpty(packageId))
            {
                await context.Forward(new DisplayPackageStatusDialog(packageId), AfterDisplayPackageStatus, context.Activity as IMessageActivity);
            }
            else
            {
                await context.PostAsync("Is there anything else I can do for you?");
                context.Wait(MessageReceived);
            }
        }


        private async Task AfterDisplayPackageStatus(IDialogContext context, IAwaitable<object> result)
        {
            await Task.FromResult<object>(null);
            
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