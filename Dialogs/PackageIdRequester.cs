using Link.Domain.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class PackageIdRequester
    {
        private IDialogContext _context;

        public PackageIdRequester(IDialogContext context)
        {
            _context = context;
        }


        public void ObtainPackageId()
        {
            PromptDialog.Text(_context, AfterPackageIdRequestAsync, "Please enter your tracking number", "Please try again", 3);
        }


        private async Task AfterPackageIdRequestAsync(IDialogContext context, IAwaitable<string> result)
        {
            var validator = WebApiApplication.IoCResolver.GetInstance<IPackageValidator>();
            var candidate = await result;

            if (validator.IsValidId(candidate))
            {
                context.ConversationData.SetValue("PackageId", candidate);
            }
        }
    }
}