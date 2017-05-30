using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Link.Domain.Contracts;
using Link.Data.File;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        IPackageManager _packageManager;
        string username;
        private readonly ISettingsReader _settings;

        public RootDialog()
        {
            _settings = new SettingsReader();
        }

        public Task StartAsync(IDialogContext context)
        {

            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task OnNameReceived(IDialogContext context, IAwaitable<string> result)
        {
            username = await result;

            var images = new[] { new CardImage("http://www.benniebos.com/hotair/mei2007/DHL.jpg", "A DHL Balloon") };
            var card = new HeroCard($"Hello, {username}", "Welcome to the DHL Bot!", "I can help you track your packages, reschedule a delivery, cancel a delivery, change delivery address and much much more! Just tell me what you want to do, and I will help you get it done.", images);

            var reply = context.MakeMessage();
            reply.Attachments.Add(card.ToAttachment());
            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            await context.Forward<object>(new PackageDialog(_settings), AfterPackageDialog, activity);

        }

        private async Task AfterPackageDialog(IDialogContext context, IAwaitable<object> result)
        {
            await Task.CompletedTask;
            context.Wait(MessageReceivedAsync);
        }

        private async Task PackageIdReceived(IDialogContext context, IAwaitable<string> result)
        {
            var packageId = await result;

            var packageInfo = await _packageManager.RetrievePackageInfoAsync(packageId);

            if (packageInfo == null)
            {

            }

        }
    }
}