using System;
namespace markdoc.Data
{
    public class Document : Realms.RealmObject
    {
        [Realms.PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Body
        {
            get;
            set;
        }

        public string Title { get; set; }
    }
}
