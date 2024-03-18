namespace SeriousGameComponents.SlideComponent
{
    public class LogActivity
    {
        public static Activity Slide { get { return new Activity("http://id.tincanapi.com/activitytype/slide", "slide"); } }
        public static Activity SlideDeck { get { return new Activity("http://id.tincanapi.com/activitytype/slide-deck", "slide-deck"); } }
        public static Activity Question { get { return new Activity("http://activitystrea.ms/schema/1.0/question", "question"); } }
        public static Activity Assessment { get { return new Activity("http://adlnet.gov/expapi/activities/assessment", "assessment"); } }
    }
}