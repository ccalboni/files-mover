using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesMover
{
    internal static class Indexer
    {
        public static List<Action> GetFilesMatchingRules(List<Rule> rules)
        {
            List<Action> actions = new List<Action>();
            foreach (var rule in rules)
            {
                Indexer.ProcessRule(rule, actions);
            }
            return actions;
        }

        private static void ProcessRule(Rule rule, List<Action> actions)
        {
            if (System.Environment.MachineName.Equals(rule.ComputerName, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.Information("Processing rule '{0}'", rule.Name);
                var foundFilePaths = new List<string>();
                Indexer.SearchFilesByExtension(rule.SourceDirectory, rule.Extensions, foundFilePaths, rule.IncludeSubdirectories);

                foreach (var foundFilePath in foundFilePaths)
                {
                    string destinationFilePath = Path.Combine(rule.DestinationDirectory, Path.GetFileName(foundFilePath));
                    actions.Add(
                        new Action()
                        {
                            SourceFilePath = foundFilePath,
                            DestinationFilePath = destinationFilePath,
                            Overwrite = rule.OverwriteDestinationFiles
                        });
                }
            }
            else
            {
                Log.Information("Rule not processed, matchine name does not match: '{0}' != '{1}'", 
                    System.Environment.MachineName, rule.ComputerName);
            }
        }

        private static void SearchFilesByExtension(string path, List<string> validExtensions, List<string> foundFiles, bool includeSubdirectories)
        {
            if (!Directory.Exists(path))
            {
                Log.Warning("Rule not processed, directory does not exists: '{0}'", path);
                return;
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                string fileExtension = Path.GetExtension(file).ToLower().Trim('.');
                if (validExtensions.Contains("*")
                    || validExtensions.Any(s => s.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)))
                {
                    foundFiles.Add(file);
                }
            }

            if (includeSubdirectories)
            {
                foreach (var directory in Directory.GetDirectories(path))
                {
                    Indexer.SearchFilesByExtension(directory, validExtensions, foundFiles, includeSubdirectories);
                }
            }
        }
    }
}
