using Link.Domain.Entities;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Threading.Tasks;

namespace LogisticBot.Forms
{
    [Serializable, Template(TemplateUsage.NotUnderstood, "Hey, I didn't understand that!")]
    public class DeliveryAddress : Address
    {
        [Prompt("Enter your street address")]
        new public string StreetAddress
        {
            get { return base.StreetAddress; }
            set { base.StreetAddress = value; }
        }


        new public string ZipCode
        {
            get { return base.ZipCode; }
            set { base.ZipCode = value; }
        }


        new public string City
        {
            get { return base.City; }
            set { base.City = value; }
        }


        new public string Country
        {
            get { return base.Country; }
            set { base.Country = value; }
        }


        [Prompt("Will there be someone at this address during daytime?{||}", ChoiceStyle = ChoiceStyleOptions.Buttons)]
        public AnswerOptions PeoplePresentDuringDayTime { get; set; }


        public static IForm<DeliveryAddress> BuildForm()
        {
            return new FormBuilder<DeliveryAddress>()
                    .Message("Allright. Where would you like your package delivered?")
                    .AddRemainingFields()

                    .Field(new FieldReflector<DeliveryAddress>(nameof(PeoplePresentDuringDayTime))
                        .SetValidate(async (state, value) => await ValidatePeoplePresentDuringDayTime(state, value)))

                    .Confirm("Change delivery address to this? {*}")
                    .Build();
        }

        private static async Task<ValidateResult> ValidatePeoplePresentDuringDayTime(DeliveryAddress state, object value)
        {
            var optionAnswered = (AnswerOptions)value;
            if (optionAnswered == AnswerOptions.Yes)
            {
                state.PeoplePresentDuringDayTime = optionAnswered;
            }
            return await Task.FromResult(new ValidateResult { IsValid = true, Value = value });
        }

        public enum AnswerOptions
        {
            NoValue,
            Yes,
            No
        }
    }
}