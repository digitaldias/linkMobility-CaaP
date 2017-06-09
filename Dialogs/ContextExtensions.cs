using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace LogisticBot.Dialogs
{
    public static class ContextExtensions
    {
        public static bool NotSureEnough(this LuisResult result)
        {
            return result.TopScoringIntent.Score <= 0.9;
        }


        public static string GetPackageId(this IDialogContext context)
        {
            if (!context.ConversationData.ContainsKey("LuisResult"))
                return string.Empty;

            var luisResult = context.ConversationData.GetValue<LuisResult>("LuisResult");

            EntityRecommendation entity;
            if (luisResult.TryFindEntity("PackageID", out entity))
                return entity.Entity;

            return string.Empty;
        }


        /// <summary>
        /// This assumes that you have stored a valid LuisResult in context.ConversationData using
        /// context.ConversationData.SetValue(). It looks for the key "LuisResult". 
        /// </summary>
        public static string FindPackageId(this IDialogContext context)
        {
            string packageId = string.Empty;
            if (context.ConversationData.TryGetValue<string>("PackageId", out packageId))
                return packageId;

            if (!context.ConversationData.ContainsKey("LuisResult"))
                return string.Empty;

            var luisResult = context.ConversationData.GetValue<LuisResult>("LuisResult");

            EntityRecommendation packageIdEntity;
            if (luisResult.TryFindEntity("PackageId", out packageIdEntity))
            {
                context.ConversationData.SetValue("PackageId", packageIdEntity.Entity);
                return packageIdEntity.Entity;
            }
            return string.Empty;
        }


        public static string FindUserName(this IDialogContext context)
        {
            if(context.UserData.ContainsKey("UserName"))
            {
                return context.UserData.GetValue<string>("UserName");
            }
            return string.Empty;
        }


        public static void SetPackageId(this IDialogContext context, string packageId)
        {
            context.ConversationData.SetValue("PackageId", packageId);
        }


        public static void SetUserName(this IDialogContext context, string username)
        {
            context.UserData.SetValue("UserName", username);
        }
    }
}