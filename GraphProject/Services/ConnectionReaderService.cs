using GraphProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphProject.Services
{
    /// <summary>
    /// Service for reading connections from input file
    /// </summary>
    public class ConnectionReaderService
    {
        public const int CountElementsOfValidString = 4;
        public const char LinesDefaultSpliter = ' ';

        public IEnumerable<ServiceConnection> ReadConnectionsInTimeFrameFromFile(string pathToFile, DateTime start, DateTime end)
        {
            if (!File.Exists(pathToFile))
            {
                Console.WriteLine($"File with name {pathToFile} not found!");
                return null;
            }

            var lines = File.ReadAllLines(pathToFile);
            List<ServiceConnection> serviceConnections = new List<ServiceConnection>();
            foreach (var line in lines)
            {
                var lineElements = line.Split(LinesDefaultSpliter);
                if (lineElements.Count() != CountElementsOfValidString)
                {
                    Console.WriteLine($"Invalid string: {line}");
                    continue;
                }

                var serviceConnection = new ServiceConnection();

                DateTime connectionDateTime = new DateTime();

                if (DateTime.TryParse($"{lineElements[0]} {lineElements[1]}", out connectionDateTime))
                {
                    serviceConnection.ConnectionDateTime = connectionDateTime;
                }
                else
                {
                    Console.WriteLine($"Invalid datetime in string: {line}");
                    continue;
                }

                if (connectionDateTime < start || connectionDateTime > end)
                {
                    continue;
                }

                serviceConnection.SenderName = lineElements[2];
                serviceConnection.ReceiverName = lineElements[3];
                serviceConnections.Add(serviceConnection);
            }

            if (!serviceConnections.Any())
            {
                Console.WriteLine($"No valid connections found in file {pathToFile}");
                return null;
            }

            return serviceConnections;
        }
    }
}
