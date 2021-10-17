using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleLogger
{

    [Serializable]
    public class ServerData
    {

        public string message = "";
        public int flag = 0; //1 == shutdown

        public static string Serialize(ServerData data)
        {

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer ser = new XmlSerializer(typeof(ServerData));

            using StringWriter textWriter = new StringWriter();
            ser.Serialize(textWriter, data, ns);
            return textWriter.ToString();
        }

        public static ServerData Deserialize(string xml)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ServerData));
            using var reader = new StringReader(xml);
            return (ServerData)ser.Deserialize(reader);
        }
    }
}
