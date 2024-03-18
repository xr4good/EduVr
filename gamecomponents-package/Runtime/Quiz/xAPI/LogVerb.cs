namespace SeriousGameComponents.QuizComponent
{
    public static class LogVerb
    {
        public static Verb Opened { get { return new Verb("http://activitystrea.ms/schema/1.0/open", "opened"); } }
        public static Verb Answered { get { return new Verb("http://adlnet.gov/expapi/verbs/answered", "answered"); } }
        public static Verb Selected { get { return new Verb("http://id.tincanapi.com/verb/selected", "selected"); } }
        public static Verb Closed { get { return new Verb("http://activitystrea.ms/schema/1.0/close", "closed"); } }
        public static Verb Completed { get { return new Verb("http://activitystrea.ms/schema/1.0/complete", "completed"); } }
    }
}