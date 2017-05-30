using Link.Domain.Contracts;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LogisticBot.Dialogs
{
    [Serializable]
    public class PackageDialog : LuisDialog<object>
    {
        public PackageDialog(ISettingsReader settings) 
            : base(new LuisService(new LuisModelAttribute(settings["luis.modelid"], settings["luis.subscriptionkey"])))
        {
        }


        [LuisIntent("Track package")]
        public async Task TrackPackage(IDialogContext context, LuisResult result)
        {
            //EntityRecommendation recomendation;

            //if (result.TryFindEntity("PackageID", out recomendation))
            //{

            //}

            await context.PostAsync("I have your package right here");

            context.Done<object>(null);
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I have no idea what you just said!");

            context.Done<object>(null);
        }

    }
}