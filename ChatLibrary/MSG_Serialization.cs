using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
namespace ChatLibrary
{
    public class MSG_Serialization
    {
        [JsonPropertyName("senderName")]
        public string senderName { get; set; }
        [JsonPropertyName("text")]
        public string text { get; set; }
        [JsonPropertyName("timeOfGetting")]
        public DateTime timeOfGetting { get; set; }
        public MSG_Serialization(string senderName, string text, DateTime timeOfGetting)
        {
            this.senderName = senderName;
            this.text = text;
            this.timeOfGetting = timeOfGetting;
        }
        public static explicit operator MSG_Serialization(MSG m)
        {
            return new MSG_Serialization(m.sender_name, m.text, m.timeOfGetting);
        }
    }
}
