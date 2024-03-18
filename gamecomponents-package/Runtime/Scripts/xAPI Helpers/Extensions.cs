namespace SeriousGameComponents
{
    public struct Extension
    {
        public string url;
        public string defaultValue;
        public string descriptionEnUS;

        public Extension(string url, string defaultValue, string descriptionEnUS)
        {
            this.url = url;
            this.defaultValue = defaultValue;
            this.descriptionEnUS = descriptionEnUS;
        }
    }
}