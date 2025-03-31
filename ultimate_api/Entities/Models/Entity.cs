using Entities.LinkModels;
using System.Collections.Concurrent;
using System.Xml;

namespace Entities.Models
{
    public class Entity
    {
        private static readonly string LinkElementName = nameof(Link);
        private static readonly string HrefPropertyName = nameof(Link.Href);
        private static readonly string MethodPropertyName = nameof(Link.Method);
        private static readonly string RelPropertyName = nameof(Link.Rel);

        private void WriteLinksToXml(string key, object value, XmlWriter writer)
        {
            writer.WriteStartElement(key);
            if (value.GetType() == typeof(List<Link>))
            {
                foreach (var val in value as List<Link>)
                {
                    writer.WriteStartElement(nameof(Link));
                    WriteLinksToXml(nameof(val.Href), val.Href, writer);
                    WriteLinksToXml(nameof(val.Method), val.Method, writer);
                    WriteLinksToXml(nameof(val.Rel), val.Rel, writer);
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteString(value.ToString());
            }
            writer.WriteEndElement();
        }
        public ConcurrentDictionary<string, object> Properties { get; set; } = new();

        public bool TryAdd(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or whitespace", nameof(key));

            return Properties.TryAdd(key, value);
        }
        public void Add(string key, List<Link> userLinks)
        {
            if (userLinks == null)
                throw new ArgumentNullException(nameof(userLinks));

            TryAdd(key, userLinks);
        }
    }
}
