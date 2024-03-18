namespace SeriousGameComponents.QuizComponent
{
    public static class LogActivity
    {
        public static Activity Question { get { return new Activity("http://activitystrea.ms/schema/1.0/question", "question"); } }
        public static Activity Assessment { get { return new Activity("http://adlnet.gov/expapi/activities/assessment", "assessment"); } }
        public static Activity ChecklistItem { get { return new Activity("http://id.tincanapi.com/activitytype/checklist-item", "checklist item"); } }
    }
}