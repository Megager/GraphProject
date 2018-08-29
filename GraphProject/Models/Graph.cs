using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphProject.Models
{
    /// <summary>
    /// DTO for graph representation
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Dictionary for nodes of graph
        /// </summary>
        public Dictionary<string, int> Nodes { get; set; }

        /// <summary>
        /// Weight matrix of graph
        /// </summary>
        public int[,] WeightMatrix { get; set; }

        public Graph()
        {
            this.Nodes = new Dictionary<string, int>();
            this.WeightMatrix = new int[0, 0];
        }

        /// <summary>
        /// Constructor which take as a param connections and create graph
        /// </summary>
        /// <param name="connections">IEnumerable of connections beetwen services</param>
        public Graph(IEnumerable<ServiceConnection> connections)
        {
            // Create dictionary of services from connections
            this.Nodes = connections.Select(x => new[] 
                                {
                                    x.ReceiverName,
                                    x.SenderName
                                })
                                .SelectMany(x => x)
                                .Distinct()
                                .Select((x, index) => new KeyValuePair<string, int>(x, index))
                                .ToDictionary(x => x.Key, x => x.Value);

            var nodesCount = Nodes.Count;
            this.WeightMatrix = new int[nodesCount, nodesCount];
            var groupedBySenderConnections = connections.GroupBy(x => x.SenderName)
                                                        .ToDictionary(x => x.Key, 
                                                                      x => x.Select(y => y.ReceiverName));

            //Creating weitgh matrix
            foreach (var service in this.Nodes)
            {
                if (groupedBySenderConnections.ContainsKey(service.Key))
                {
                    foreach (var serviceConnection in groupedBySenderConnections[service.Key])
                    {
                        this.WeightMatrix[service.Value, this.Nodes[serviceConnection]]++;
                        this.WeightMatrix[this.Nodes[serviceConnection], service.Value]++;
                    }
                }
            }
        }

        public int GetServiceConnectionsCount(string serviceName)
        {
            if (!this.Nodes.ContainsKey(serviceName))
            {
                Console.WriteLine($"Service with name {serviceName} not found!");
                return 0;
            }

            int result = 0;
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                result += this.WeightMatrix[this.Nodes[serviceName], i];
            }

            return result;
        }

        public int GetConnectionsCountBetweenServices(string senderService, string receiverService)
        {
            if (!this.Nodes.ContainsKey(senderService))
            {
                Console.WriteLine($"Service with name {senderService} not found!");
                return 0;
            }

            if (!this.Nodes.ContainsKey(receiverService))
            {
                Console.WriteLine($"Service with name {receiverService} not found!");
                return 0;
            }

            return this.WeightMatrix[this.Nodes[senderService], this.Nodes[receiverService]];
        }
    }
}
