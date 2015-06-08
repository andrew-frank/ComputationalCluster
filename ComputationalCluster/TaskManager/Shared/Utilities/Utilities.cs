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
using ComputationalCluster.Nodes;
using ComputationalCluster.Misc;


namespace ComputationalCluster.Shared.Utilities {

    public static class Utilities
    {
        #region encoding

        public static byte[] Base64Encode(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

            //var bytes = Encoding.UTF8.GetBytes(str);
            //string base64 = Convert.ToBase64String(bytes);
            //return base64;
            //Console.WriteLine(base64);
        }

        public static string Base64Decode(byte[] bytes) //byte[] bytes
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);

            //var data = Convert.FromBase64String(bytes);
            //var str = Encoding.UTF8.GetString(data);
            //return str;
        }

        #endregion


        #region Misc

        public static string ProblemNameForType(ProblemType t)
        {
            switch (t) {
                case ProblemType.DVRP: return "DVRP";
                default: return null;
            }
        }

        public static string NodeNameForType(NodeType t)
        {
            switch (t) {
                case NodeType.Server: return "CommunicationServer";
                case NodeType.Client: return "Client";
                case NodeType.ComputationalNode: return "ComputationalNode";
                case NodeType.TaskManager: return "TaskManager";
                default: return null;
            }
        }

        public static NodeType NodeTypeForName(string t)
        {
            switch (t) {
                case "CommunicationServer": return NodeType.Server;
                case "Client": return NodeType.Client;
                case "ComputationalNode": return NodeType.ComputationalNode;
                case "TaskManager": return NodeType.TaskManager;
                default: return NodeType.Unspecified;
            }
        }

        #endregion

        #region Serialization


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

        public static Object DeserializeXMLString(this String XMLString)
        {
            return ((string)XMLString).DeserializeXML();
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

        #endregion

        public static Node PromptUserForNodeAddress()
        {
            Server node = new Server();

            while (node.Port == 0)
            {
                Console.WriteLine(" Parameters Syntax: [-address [IPv4 address or IPv6 address or host name]] [-port[port number]]");
                Console.Write("> ");

                String parameters;
                parameters = Console.ReadLine();
                parameters = parameters.Replace(" ", string.Empty);
                //Shared.Connection.ConnectionService.CheckInputSyntax(parameters, Port, HostName);
            }

            return node;
        }
    }
}