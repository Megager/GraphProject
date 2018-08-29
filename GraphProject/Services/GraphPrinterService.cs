using GraphProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphProject.Services
{
    /// <summary>
    /// Service for printing graph
    /// </summary>
    public class GraphPrinterService
    {
        public string GetFullGraphDescription(DateTime start, DateTime end, Graph graph)
        {
            var result = new StringBuilder();
            result.Append(this.PrintGraph(graph));
            result.Append(this.GetStatistics(graph));
            result.Append(this.GetServiceConnectionsPerSecondInInterval(start, end, graph));

            return result.ToString();
        }

        public string PrintGraph(Graph graph)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Graph indexes:");
            foreach (var node in graph.Nodes)
            {
                stringBuilder.AppendLine($"{node.Key} index: {node.Value + 1}");
            }

            stringBuilder.AppendLine("WeightMatrix:");
            stringBuilder.Append("  ");
            var nodesCount = graph.Nodes.Count;
            foreach (var node in graph.Nodes)
            {
                stringBuilder.Append($" {node.Value + 1}");
            }

            stringBuilder.Append(Environment.NewLine);

            foreach (var node in graph.Nodes)
            {
                var index = node.Value;
                stringBuilder.Append($"{index + 1}:");
                for (int i = 0; i < nodesCount; i++)
                {
                    stringBuilder.Append($" {graph.WeightMatrix[index, i]}");
                }

                stringBuilder.Append(Environment.NewLine);
            }


            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }

        public string GetStatistics(Graph graph)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Statistics:");
            foreach (var node in graph.Nodes)
            {
                stringBuilder.AppendLine($"{node.Key} service count of connections: {graph.GetServiceConnectionsCount(node.Key)}");
            }

            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }

        public string GetServiceConnectionsPerSecondInInterval(DateTime start, DateTime end, Graph graph)
        {
            var interval = end - start;
            var secondsInInterval = interval.TotalSeconds;
            var result = new StringBuilder();
            result.AppendLine();
            result.AppendLine($"Service connecttions per second in interval {start}  -  {end}");
            foreach (var node in graph.Nodes.Keys)
            {
                result.AppendLine($"{node}:{graph.GetServiceConnectionsCount(node) / secondsInInterval}");
            }

            return result.ToString();
        }
    }
}
