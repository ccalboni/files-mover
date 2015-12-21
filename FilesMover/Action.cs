using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesMover
{
    internal class Action
    {
        public string SourceFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        public bool Overwrite { get; set; }
    }
}
