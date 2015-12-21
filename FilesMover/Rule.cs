using System;
using System.Collections.Generic;

namespace FilesMover
{
    public class Rule
    {
        public string Name { get; set; }

        public string ComputerName { get; set; }

        public string SourceDirectory { get; set; }

        public string DestinationDirectory { get; set; }

        public bool IncludeSubdirectories { get; set; }

        public List<string> Extensions { get; set; }

        public bool OverwriteDestinationFiles { get; set; }
    }
}


