using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CsharpSandbox.XmlSerialization
{
    public class SerializationSandbox : ISandboxRunner
    {
        public void Run()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SimpleModel));

            var xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore
            };
            using var stringReader = new StringReader(@"<?xml version=""1.0"" encoding=""UTF - 8"" standalone=""yes""?><!DOCTYPE SimpleModel SYSTEM ""TEST.DTD"">
<SimpleModel><SimpleString>123</SimpleString></SimpleModel>");
            using var xmlReader = XmlReader.Create(stringReader, xmlReaderSettings);

            serializer.Deserialize(xmlReader);
        }
    }
}
