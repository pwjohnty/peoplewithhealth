﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
namespace PeopleWith
{
    public class QuestionnaireAnswerConverter : JsonConverter
    {
        public ObservableCollection<QuestionAnswers> FeedbackSymptoms = new ObservableCollection<QuestionAnswers>();
        public string ReturnString;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObservableCollection<QuestionAnswers>);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                JToken token = JToken.Load(reader);
                if (token.Type == JTokenType.String)
                {
                    String value = token.Value<string>();
                    if (value.Contains("id"))
                    {
                        //Add Foreach here to accomdate multiple symptom feedbacks 
                        var Fullstring = value.Split('{', '}');
                        char[] charsToRemove = new char[] { '[', '{', '}', ']', '"' };
                        foreach (var c in charsToRemove)
                        {
                            ReturnString = Fullstring[1].Replace(c.ToString(), string.Empty);
                        }

                        var SectionSplit = ReturnString.Split(',');
                        var timesubstring = SectionSplit[0].Split(":");
                        //var Timestamp = SectionSplit[0].Split(":");
                        var SymFeedback = SectionSplit[1].Split(":");
                        var Intensity = SectionSplit[2].Split(":");
                        var Notes = SectionSplit[3].Split(":");
                        var Triggerss = SectionSplit[4].Split(":");
                        var Interventions = SectionSplit[5].Split(":");
                        var Duration = SectionSplit[6].Split(":");
                        var AddFeedback = new QuestionAnswers();
                        AddFeedback.answerid = timesubstring[1];
                        AddFeedback.answertitle = SymFeedback[1];
                        AddFeedback.answervalue = Intensity[1];

                        //AddFeedback.triggers = Triggerss[1];
                        //AddFeedback.interventions = Interventions[1];
                        //  AddFeedback.duration = Duration[1];
                        FeedbackSymptoms.Add(AddFeedback);
                        return FeedbackSymptoms;
                    }
                    else
                    {
                        return new ObservableCollection<QuestionAnswers>();
                    }
                }
                else if (token.Type == JTokenType.Null)
                {
                    return new ObservableCollection<QuestionAnswers>();
                }
                throw new JsonSerializationException($"Unexpected token type: {token.Type}");
            }
            catch (Exception ex)
            {
                return null;
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