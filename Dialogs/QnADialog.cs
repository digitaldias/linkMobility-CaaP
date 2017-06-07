using Link.Domain.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using QnAMakerDialog;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class QnADialog : QnAMakerDialog<object>
    {
        public QnADialog(ISettingsReader settings)
        {
            base.SubscriptionKey = settings["QnaMaker.SubscriptionKey"];
            base.KnowledgeBaseId = settings["QnaMaker.KnowledgeBaseId"];
        }


        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            // TODO: Pass utterance to LUIS Dialog
            await context.PostAsync($"Sorry, I couldn't find an answer for '{originalQueryText}'.");
            context.Wait(MessageReceived);
        }


        [QnAMakerResponseHandler(90)]
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            await context.PostAsync($"I found an answer that might help...{result.Answer}.");
            context.Wait(MessageReceived);
        }
    }
}