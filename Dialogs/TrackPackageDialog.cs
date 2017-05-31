using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;

namespace LogisticBot.Dialogs
{
    internal class TrackPackageDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            
            var luisResult = context.ConversationData.GetValue<LuisResult>("LuisResult");
            luisResult.Entities.try

            context.Wait(MessageReceivedAsync);
        }


        private Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            throw new NotImplementedException();
        }
    }
}