namespace SeriousGameComponents.AudioPlayerComponent
{
    public class LogVerb
    {
        public static Verb Started { get { return new Verb("https://activitystrea.ms/schema/1.0/start", "started"); } }
        public static Verb Resumed { get { return new Verb("http://adlnet.gov/expapi/verbs/resumed", "resumed"); } }
        public static Verb Paused { get { return new Verb("http://id.tincanapi.com/verb/paused", "paused"); } }
        public static Verb Canceled { get { return new Verb("http://activitystrea.ms/schema/1.0/cancel", "canceled"); } }
        public static Verb Listened { get { return new Verb("http://activitystrea.ms/schema/1.0/listen", "listened"); } }
        public static Verb Returned { get { return new Verb("http://activitystrea.ms/schema/1.0/return", "returned"); } }
        public static Verb Skipped { get { return new Verb("http://id.tincanapi.com/verb/skipped", "skipped"); } }


    }
}