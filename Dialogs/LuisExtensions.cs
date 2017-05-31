using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace LogisticBot.Dialogs
{
    public static class LuisExtensions
    {
        public static bool NotSureEnough(this LuisResult result)
        {
            return result.TopScoringIntent.Score <= 0.9;
        }


        public static bool TryToExtractPackageIdFromLuisResult(this IDialogContext context)
        {
            var luisResult = context.ConversationData.GetValue<LuisResult>("LuisResult");

            EntityRecommendation packageIdEntity;
            if (luisResult.TryFindEntity("PackageID", out packageIdEntity))
            {
                context.ConversationData.SetValue<string>("PackageID", packageIdEntity.Entity);
                return true;
            }
            return false;
        }
    }
}