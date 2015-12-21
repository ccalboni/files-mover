using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FilesMover
{
    internal static class Mover
    {
        public static void Move(List<Action> actions)
        {
            foreach (var action in actions)
            {
                Mover.ExecuteAction(action);
            }
        }

        private static void ExecuteAction(Action action)
        {
            var destinationDirectory = Path.GetDirectoryName(action.DestinationFilePath);
            if (!Directory.Exists(destinationDirectory))
            {
                try
                {
                    Directory.CreateDirectory(destinationDirectory);
                    Log.Information("Destination directory correctly created: '{0}'", destinationDirectory);
                }
                catch (Exception ex)
                {
                    Log.Error("Exception while creating destination directory: '{0}'", destinationDirectory);
                    Log.Error(ex);
                    return;
                }
            }

            //when a file should be overwritten, it's initially renamed and than deleted once new one is succesfully moved 
            //in case of error, the temporary file is recovered
            string temporarilyMovedFile = null;

            if (File.Exists(action.DestinationFilePath))
            {
                if (!action.Overwrite)
                {
                    Log.Warning("Destination file exists, rule set to NOT overwrite it: '{0}'", action.DestinationFilePath);
                    return;
                }
                else
                {
                    temporarilyMovedFile = action.DestinationFilePath + ".backup";
                    try
                    {
                        FileInfo destinationFileInfo = new FileInfo(action.DestinationFilePath);
                        destinationFileInfo.MoveTo(temporarilyMovedFile);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Exception while trying to rename to a backup copy the existing destination file: '{0}'", action.DestinationFilePath);
                        Log.Error(ex);
                        return;
                    }
                }
            }

            //move that file
            try
            {
                FileInfo fileinfo = new FileInfo(action.SourceFilePath);
                fileinfo.MoveTo(action.DestinationFilePath);
                Log.Information("File successfully moved: '{0}' => '{1}'", action.SourceFilePath, action.DestinationFilePath);
                if (temporarilyMovedFile != null)
                {
                    File.Delete(temporarilyMovedFile);
                    Log.Information("Backup copy successfully deleted '{0}'", temporarilyMovedFile);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception while moving file: '{0}' => '{1}'", action.SourceFilePath, action.DestinationFilePath);
                Log.Error(ex);
                if (temporarilyMovedFile != null)
                {
                    try
                    {
                        FileInfo recoverFileInfo = new FileInfo(temporarilyMovedFile);
                        var originalFilePath = temporarilyMovedFile.Remove(temporarilyMovedFile.Length - ".backup".Length);
                        recoverFileInfo.MoveTo(originalFilePath);
                        Log.Information("Original file successfully recovered: '{0}'", originalFilePath);
                    }
                    catch (Exception recoverEx)
                    {
                        Log.Warning("Can't rename backup copy to original file name: '{0}'", temporarilyMovedFile);
                        Log.Error(recoverEx);
                    }
                }
                return;
            }
        }
    }
}
