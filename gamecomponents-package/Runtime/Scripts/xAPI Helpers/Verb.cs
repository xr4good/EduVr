namespace SeriousGameComponents
{
    public struct Verb
    {
        public string url;
        public string descriptionEnUS;

        public Verb(string url, string descriptionEnUS)
        {
            this.url = url;
            this.descriptionEnUS = descriptionEnUS;
        }
    }
}