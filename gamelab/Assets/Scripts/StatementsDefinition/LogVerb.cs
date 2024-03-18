public struct Verb
{
    public string url;
    public string descriptionEnUS;
    public string descriptionPtBR;

    public Verb(string url, string descriptionEnUS, string descriptionPtBR)
    {
        this.url = url;
        this.descriptionEnUS = descriptionEnUS;
        this.descriptionPtBR = descriptionPtBR;
    }
}


public class LogVerb
{

    public static Verb Viewed   { get { return new Verb("http://id.tincanapi.com/verb/viewed", "viewed", "visualizou"); } }
    public static Verb Completed   { get { return new Verb("http://activitystrea.ms/schema/1.0/complete", "completed", "completou"); } }
    public static Verb Answered   { get { return new Verb("http://adlnet.gov/expapi/verbs/answered", "answered", "respondeu"); } }
}