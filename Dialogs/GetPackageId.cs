using Microsoft.Bot.Builder.Dialogs;
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
            await Task.CompletedTask;            

            _userName    = context.FindUserName();
            _packageId   = context.FindPackageId();

            if(!string.IsNullOrEmpty(_packageId))
            {                
                context.Call(new ConfirmPackageIdResuse(_packageId), AfterConfirmIdReuseAsync);
            }
            else
            {
                context.Call<string>(new AskForPackageId(), AfterAskingForPackageIdAsync);
            }
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
                context.Call(new AskForPackageId(), AfterAskingForPackageIdAsync);                
            }
        }


        private async Task AfterAskingForPackageIdAsync(IDialogContext context, IAwaitable<string> result)
        {
            
            context.Done(await result);
        }
    }
}