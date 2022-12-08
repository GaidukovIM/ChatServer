using System.Text.Json;

namespace ChatLibrary
{
    public class MSG
    {
        internal string sender_name;
        internal string text;
        internal DateTime timeOfGetting;
        public MSG(string sender_name, string text, DateTime timeOfGetting)
        {
            this.sender_name = sender_name;
            this.text = text;
            this.timeOfGetting = timeOfGetting;
        }
        public static explicit operator MSG(MSG_Serialization ms)
        {
            return new MSG(ms.senderName, ms.text, ms.timeOfGetting);
        }
        public override string ToString()
        {
            return $"{timeOfGetting.GetDateTimeFormats()} {sender_name}: {text}";
        }
        public static List<MSG> GetAllMSGs(string jsonFileName)
        {
            List<MSG> msgs = new List<MSG>();
            string s;
            using(var reader=new StreamReader(jsonFileName))
            {
                s= reader.ReadToEnd();
                if (s == "" || s == "{}") return msgs;
            }
            msgs = JsonSerializer.Deserialize<List<MSG>>(s);

            return msgs;
        }
        public void WriteMsgToFile(string jsonFileName)
        {
            using(var writer=new StreamWriter(jsonFileName))
            {
                writer.Write(JsonSerializer.Serialize<MSG_Serialization>((MSG_Serialization)this));
            }
        }
    }
}