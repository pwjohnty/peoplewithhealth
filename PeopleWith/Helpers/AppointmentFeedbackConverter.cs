﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;

namespace PeopleWith
{
    public class AppointmentFeedbackConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObservableCollection<appointmentfeedback>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                JToken token = JToken.Load(reader);

                if (token.Type == JTokenType.String)
                {
                    string jsonString = token.ToString();
                    try
                    {
                        return JsonConvert.DeserializeObject<ObservableCollection<appointmentfeedback>>(jsonString);
                    }
                    catch (JsonException)
                    {
                        return new ObservableCollection<appointmentfeedback>();
                    }
                }
                else if (token.Type == JTokenType.Null)
                {
                    return new ObservableCollection<appointmentfeedback>();
                }
                else
                {

                    return new ObservableCollection<appointmentfeedback>();
                }

                //throw new JsonSerializationException($"Unexpected token type: {token.Type}");
            }
            catch (Exception Ex)
            {
                //return new ObservableCollection<appointmentfeedback>();
                return null;
            }
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            JToken t = JToken.FromObject(value);
            t.WriteTo(writer);
        }
    }
}