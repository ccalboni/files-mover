using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace FilesMover
{
    class Program
    {
        public static void Main(string[] args)
        {
            Log.Information("************************************************************");
            Log.Information("FilesMover v.{0} (on {1})", Assembly.GetExecutingAssembly().GetName().Version.ToString(), System.Environment.MachineName);
            Log.Information("************************************************************");
            Log.Information("");

            //load rules
            var rules = Program.LoadRules();

            if (rules.Count > 0)
            {
                //index files
                var actions = Indexer.GetFilesMatchingRules(rules);

                if (actions.Count > 0)
                {
                    //move
                    Mover.Move(actions);
                }
            }

            //end
            Log.Information("");
            Log.Information("Program ended");
            Log.Information("");
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static List<Rule> LoadRules()
        {
            var rulesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rules.json");
            var rules = new List<Rule>();


            if (!File.Exists(rulesFilePath))
            {
                Log.Warning("Rules file not found: '{0}'", rulesFilePath);
            }
            else
            {
                try
                {
                    var rulesFileContent = File.ReadAllText(rulesFilePath);
                    rules = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Rule>>(rulesFileContent);
                    Log.Information("Rules file successfully loaded");
                }
                catch (Exception ex)
                {
                    Log.Error("Exception while loading rules from file: '{0}'", rulesFilePath);
                    Log.Error(ex);
                }
            }

            return rules;

        }

    }
}
