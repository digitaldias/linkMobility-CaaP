using Microsoft.Bot.Builder.Luis.Models;

namespace LogisticBot.Dialogs
{
    public static class LuisExtensions
    {
        public static bool NotSureEnough(this LuisResult result)
        {
            return result.TopScoringIntent.Score <= 0.9;
        }
    }
}