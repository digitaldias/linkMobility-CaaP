using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class ConfirmPackageIdResuse : IDialog<bool>
    {
        private string _packageId;


        public ConfirmPackageIdResuse(string packageId)
        {
            _packageId = packageId;
        }


        public async Task StartAsync(IDialogContext context)
        {
            await Task.CompletedTask;
            PromptDialog.Confirm(context, AfterConfirmationAsync, $"Shall I reuse package {_packageId}?", "Oops, try again", 3, PromptStyle.Auto);                     
        }


        private async Task AfterConfirmationAsync(IDialogContext context, IAwaitable<bool> result)
        {
            var reuse = await result;
            context.Done(reuse);

        }
    }
}