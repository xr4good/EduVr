namespace SeriousGameComponents.QuizComponent
{
    //Extension (url, defaultValue, descriptionEnUS)
    public class LogExtensions
    {
        public static Extension AttemptID { get { return new Extension("http://id.tincanapi.com/extension/attempt-id", "default_id", "attemptID"); } }
    }
}
