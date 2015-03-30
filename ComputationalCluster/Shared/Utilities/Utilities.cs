using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Shared.Utilities {
    public static class Utilities 
    {
        public static string SerializeToXML<T>(this T value) {
            if (value == null) {
                return string.Empty;
            }
            try {
                var xmlserializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var stringWriter = new System.IO.StringWriter();
                using (var writer = System.Xml.XmlWriter.Create(stringWriter)) {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            } catch (Exception ex) {
                throw new Exception("An error occurred", ex);
            }
        }


        public static Object DeserializeXML(this string XMLString, Type type)
        {
            System.Xml.Serialization.XmlSerializer oXmlSerializer = new System.Xml.Serialization.XmlSerializer(type); 
            
            //The StringReader will be the stream holder for the existing XML file 
            Object YourClassObject = oXmlSerializer.Deserialize(new System.IO.StringReader(XMLString)); 
            
            //initially deserialized, the data is represented by an object without a defined type 
            return YourClassObject;
        }

        public static void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

    }
}