namespace MSC.CodeGenHero.DTO
{
    public class NamespaceItem
    {
        public NamespaceItem()
        {
        }

        public NamespaceItem(string namespaceString)
        {
            this.Namespace = namespaceString;
        }

        public NamespaceItem(string namespacePrefix, string namespaceString)
        {
            this.NamespacePrefix = namespacePrefix;
            this.Namespace = namespaceString;
        }

        public string Name { get; set; }
        public string Namespace { get; set; }
        public string NamespacePrefix { get; set; }
    }
}