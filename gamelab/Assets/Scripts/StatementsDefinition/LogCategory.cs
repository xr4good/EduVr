public struct Category
{
    public string url;
    public string descriptionEnUS;
    public string descriptionPtBR;

    public Category(string url, string descriptionEnUS, string descriptionPtBR)
    {
        this.url = url;
        this.descriptionEnUS = descriptionEnUS;
        this.descriptionPtBR = descriptionPtBR;
    }
}

public class LogCategory
{

    public static Category Slide   { get { return new Category("http://id.tincanapi.com/activitytype/slide", "slide", "página de slide"); } }
    public static Category SlideDeck   { get { return new Category("http://id.tincanapi.com/activitytype/slide-deck", "slide-deck", "documento de slides"); } }
    public static Category Question { get { return new Category("http://activitystrea.ms/schema/1.0/question", "question", "pergunta"); }}
    public static Category Assessment { get { return new Category("http://adlnet.gov/expapi/activities/assessment", "assessment", "teste"); }}
}