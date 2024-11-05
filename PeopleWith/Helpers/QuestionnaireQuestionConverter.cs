using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
namespace PeopleWith
{
    public class QuestionnaireQuestionConverter : JsonConverter
    {
        public ObservableCollection<questionanswerinfo> FeedbackSymptoms = new ObservableCollection<questionanswerinfo>();
        public string ReturnString;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObservableCollection<questionanswerinfo>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                // Load the JSON token
                JToken token = JToken.Load(reader);

                // Check if the token is a valid array
                if (token.Type == JTokenType.Array)
                {
                    // Deserialize the array into ObservableCollection<questionanswerinfo>
                    var items = token.ToObject<ObservableCollection<questionanswerinfo>>(serializer);
                    return items;
                }
                else if (token.Type == JTokenType.Null)
                {
                    // Return an empty collection if the token is null
                    return new ObservableCollection<questionanswerinfo>();
                }

                throw new JsonSerializationException("Expected an array or null token.");
            }
            catch(Exception ex)
            {
                return new ObservableCollection<questionanswerinfo>();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Use the default serialization logic
            JToken t = JToken.FromObject(value);
            t.WriteTo(writer);
        }
    }
}