using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphProject.Models
{
    /// <summary>
    /// DTO for service connection information
    /// </summary>
    public class ServiceConnection
    {
        public DateTime ConnectionDateTime { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}
