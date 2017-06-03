using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    internal class GetPackageId : IDialog<string>
    {
        private string _packageId;
        private string _userName;


        public async Task StartAsync(IDialogContext context)
        {            
            _userName    = context.FindUserName();
            _packageId   = context.FindPackageId();

            if(!string.IsNullOrEmpty(_packageId))
            {                
                context.Call(new ConfirmPackageIdResuse(_packageId), AfterConfirmIdReuseAsync);
            }
            else
            {
                context.Call(new AskForPackageId(), DisplayPackageStatusAsync);
            }
            await Task.CompletedTask;            
        }


        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            context.Wait(MessageReceivedAsync);
        }


        private async Task AfterConfirmIdReuseAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync("Allrighy then!");
                context.Done(_packageId);
            }
            else
            {
                context.Call(new AskForPackageId(), DisplayPackageStatusAsync);                
            }
        }



        private async Task DisplayPackageStatusAsync(IDialogContext context, IAwaitable<string> result)
        {
            _packageId = await result;
            context.Call(new DisplayPackageStatus(_packageId), AfterDisplayPackageStatus);
        }


        private async Task AfterDisplayPackageStatus(IDialogContext context, IAwaitable<object> result)
        {
            await result;
            context.Done(_packageId);
        }
    }
}