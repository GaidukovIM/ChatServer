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
    }
}