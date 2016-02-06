using System;
using System.IO;
using IWshRuntimeLibrary;

namespace USD.WordExport
{
    public static class ExportDirectoryCreator
    {
        public static string EnsureDirectory()
        {
            var direcName = "Узи молочной железы";
            var direc = new DirectoryInfo(direcName);
            if (!direc.Exists)
            {
                direc.Create();
                var shell = new WshShell();
                var shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + direcName;
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = direc.FullName;
                shortcut.Save();
            }
            return direc.FullName;
        }
    }
}