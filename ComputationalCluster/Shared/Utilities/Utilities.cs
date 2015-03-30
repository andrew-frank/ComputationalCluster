using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;
using ComputationalCluster.Shared.Messages.NoOperationNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Messages.RegisterResponseNamespace;
using ComputationalCluster.Shared.Messages.SolutionsNamespace;
using ComputationalCluster.Shared.Messages.SolvePartialProblemsNamespace;
using ComputationalCluster.Shared.Messages.SolveRequestNamespace;
using ComputationalCluster.Shared.Messages.SolveRequestResponseNamespace;
using ComputationalCluster.Shared.Messages.StatusNamespace;


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


        public static Object DeserializeXML(this string XMLString)
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(DivideProblem));

            System.Xml.Serialization.XmlSerializer oXmlSerializer;

            foreach(Type type in types) {
                try {
                    oXmlSerializer = new System.Xml.Serialization.XmlSerializer(type);

                    //The StringReader will be the stream holder for the existing XML file 
                    Object YourClassObject = oXmlSerializer.Deserialize(new System.IO.StringReader(XMLString));

                    //initially deserialized, the data is represented by an object without a defined type 
                    return YourClassObject;

                } catch (Exception e) {
                    Console.WriteLine("XML parsing: Catching wrong type exception while searching for object. \n" + e.ToString());
                }
            }

            return null;
        }

        
    }
}