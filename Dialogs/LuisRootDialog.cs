using Link.Domain.Contracts;
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
            await context.Forward<object>(new TrackPackageDialog(), AfterTrackPackageDialog, context.Activity as IMessageActivity);
        }


        private async Task AfterTrackPackageDialog(IDialogContext context, IAwaitable<object> result)
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