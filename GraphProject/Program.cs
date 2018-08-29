using GraphProject.Models;
using GraphProject.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphProject
{
    /*
      Example of input:
      Input.txt
      2018-1-1
      2019-1-1
     */
    class Program
    {
        static void Main(string[] args)
        {
            var readerService = new ConnectionReaderService();
            var printer = new GraphPrinterService();

            string filePath = null;
            while (filePath == null)
            {
                filePath = LoadFile();
            }

            DateTime? start = null;
            while (start == null)
            {
                start = LoadDateTime("start");
            }

            DateTime? end = null;
            while (end == null)
            {
                end = LoadDateTime("end");
            }

            var connections = readerService.ReadConnectionsInTimeFrameFromFile(filePath, (DateTime)start, (DateTime)end);
            if (connections == null)
            {
                Console.WriteLine($"No connections found in interval beetwen {start} and {end}");
                Console.ReadLine();
                return;
            }

            var graph = new Graph(connections);
            Console.WriteLine("Your graph and statistics:");
            Console.WriteLine(printer.GetFullGraphDescription((DateTime)start, (DateTime)end, graph));
            Console.ReadLine();
        }

        static string LoadFile()
        {
            Console.WriteLine("Please, write file name with extension which contain information about connections:");
            var filePath = Console.ReadLine();
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File with name {filePath} not found!");
                return null;
            }
            
            return filePath;
        }

        static DateTime? LoadDateTime(string typeOfTimeName)
        {
            Console.WriteLine($"Please, write {typeOfTimeName} interval:");
            var timeString = Console.ReadLine();
            DateTime dateTime = new DateTime();
            if (DateTime.TryParse(timeString, out dateTime))
            {
                Console.WriteLine($"Your input:{dateTime.ToString()}");
                return dateTime;
            }
            else
            {
                Console.WriteLine($"Invalid input, try again");
                return null;
            }
        }
    }
}
