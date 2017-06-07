using Link.Domain.Entities;
using Microsoft.Bot.Builder.FormFlow;
using System;

namespace LogisticBot.Forms
{
    [Serializable, Template(TemplateUsage.NotUnderstood, "I'm sorry, I didn't quite catch that")]
    public class DeliveryAddress : Address
    {
        [Prompt("Let's begin with filling out the street address")]
        new public string StreetAddress
        {
            get { return base.StreetAddress; }
            set { base.StreetAddress = value; }
        }

        [Prompt("Next, enter the desired zipcode")]
        new public string ZipCode
        {
            get { return base.ZipCode; }
            set { base.ZipCode = value; }
        }

        [Prompt("Which city would that be in?")]
        new public string City
        {
            get { return base.City; }
            set { base.City = value; }
        }

        [Prompt("And finally, the name of the country:")]
        new public string Country
        {
            get { return base.Country; }
            set { base.Country = value; }
        }


        [Prompt("That's it. Will there be someone at this address during daytime?{||}", ChoiceStyle = ChoiceStyleOptions.Buttons)]
        public bool PeoplePresentDuringDayTime { get; set; }


        public static IForm<DeliveryAddress> BuildForm()
        {
            return new FormBuilder<DeliveryAddress>()
                    .Message("Allright. Where would you like your package delivered? (type 'help' if you get stuck)")
                    .AddRemainingFields()                    
                    .Confirm("Change delivery address to this? {*}")
                    .Build();
        }
    }
}