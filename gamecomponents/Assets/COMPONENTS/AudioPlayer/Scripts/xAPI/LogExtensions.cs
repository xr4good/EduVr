namespace AudioPlayerComponent
{
    //Extension (url, defaultValue, descriptionEnUS)
    public class LogExtensions
    {
        public static Extension Duration { get { return new Extension("http://id.tincanapi.com/extension/duration", "0", "duration"); } }
    }
}
