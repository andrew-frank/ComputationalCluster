using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Shared.Messages.DivideProblemNamespace;
using ComputationalCluster.Shared.Messages.NoOperationNamespace;
using ComputationalCluster.Shared.Messages.RegisterNamespace;
using ComputationalCluster.Shared.Messages.RegisterResponseNamespace;
using ComputationalCluster.Shared.Messages.SolutionRequestNamespace;
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
            types.Add(typeof(NoOperation));
            types.Add(typeof(Register));
            types.Add(typeof(RegisterResponse));
            types.Add(typeof(SolutionRequest));
            types.Add(typeof(Solutions));
            types.Add(typeof(SolvePartialProblems));
            types.Add(typeof(SolveRequest));
            types.Add(typeof(SolveRequestResponse));
            types.Add(typeof(Status));

            System.Xml.Serialization.XmlSerializer xmlSerializer;

            foreach(Type type in types) {
                
                try {
                    xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
            
                    //The StringReader will be the stream holder for the existing XML file 
                    Object YourClassObject = xmlSerializer.Deserialize(new System.IO.StringReader(XMLString)); 

                    //initially deserialized, the data is represented by an object without a defined type 
                    return YourClassObject;

                } catch (System.InvalidOperationException e) {
                    System.Diagnostics.Debug.WriteLine("***\nThis is normal. Error in XML parsing: Catching wrong type exception while searching for object. \n" + e.ToString());
                }
            }

            return null;
        }

        public static void waitUntilUserClose()
        {
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

    }
}