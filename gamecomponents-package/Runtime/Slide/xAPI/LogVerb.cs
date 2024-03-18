namespace SeriousGameComponents.SlideComponent
{
    public class LogVerb
    {
        public static Verb Viewed { get { return new Verb("http://id.tincanapi.com/verb/viewed", "viewed"); } }
        public static Verb Completed { get { return new Verb("http://activitystrea.ms/schema/1.0/complete", "completed"); } }
        public static Verb Answered { get { return new Verb("http://adlnet.gov/expapi/verbs/answered", "answered"); } }
    }
}