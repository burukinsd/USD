using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace USDUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-F") || args.Contains("-f"))
            {
                Update();
            }
            else
            {
                var currentVersion = Assembly.LoadFile($"{Directory.GetCurrentDirectory()}\\USD.exe").GetName().Version;
                var getUpdateVersionTask = GetUpdateVersion();
                getUpdateVersionTask.Wait();
                var updateVersion = Version.Parse(getUpdateVersionTask.Result);
                if (currentVersion < updateVersion)
                {
                    Update();
                }
            }
            Process.Start("USD.exe");
        }

        private static void Update()
        {
            string tmpDirectory = EnsureTmpDiresctory();
            DownloadUpdateFile(tmpDirectory);

            KillProc();

            var backupDirec = new DirectoryInfo(tmpDirectory).GetDirectories("Backup").FirstOrDefault();
            if (backupDirec != null)
            {
                Directory.Delete(backupDirec.FullName, true);
                new DirectoryInfo(tmpDirectory).CreateSubdirectory("Backup");
            }

            MakeBackup(tmpDirectory);

            try
            {
                UnZipUpdate(tmpDirectory);
            }
            catch (Exception ex)
            {
                RestoreBackup(tmpDirectory);
            }
        }

        private static void RestoreBackup(string tmpDirectory)
        {
            var curretnDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var backupDirec = new DirectoryInfo(tmpDirectory).GetDirectories("Backup").First();
            DirectoryCopy(backupDirec.FullName, curretnDirectory.FullName, true, new List<string>() { System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName });
        }

        private static void KillProc()
        {
            var proc = Process.GetProcessesByName("USD");
            if (proc.Any())
            {
                proc[0].Kill();
            }
        }

        private static void UnZipUpdate(string tmpDirectory)
        {
            using (var zip = ZipFile.Open($"{tmpDirectory}\\USD.zip", ZipArchiveMode.Read))
            {
                zip.ExtractToDirectory(Directory.GetCurrentDirectory(), true);
            }
        }

        private static void MakeBackup(string tmpDirectory)
        {
            var curretnDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var backupDirec = new DirectoryInfo(tmpDirectory).GetDirectories("Backup").First();
            DirectoryCopy(curretnDirectory.FullName, backupDirec.FullName, true, new List<string>() { System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName });
        }

        private static void DownloadUpdateFile(string tmpDirectory)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile("http://usdupdate.azurewebsites.net/install/USD.zip", $"{tmpDirectory}/USD.zip");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удалось скачать файл обновления.");
                }
            }
        }

        private static string EnsureTmpDiresctory()
        {
            var directoryName = "Temp";
            var direc = new DirectoryInfo(directoryName);
            if (!direc.Exists)
            {
                direc.Create();
            }
            if (!direc.GetDirectories("Backup").Any())
            {
                direc.CreateSubdirectory("Backup");
            }
            return direc.FullName;
        }

        private static async Task<string> GetUpdateVersion()
        {
            string responseString = String.Empty;
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync("http://usdupdate.azurewebsites.net/install/version.htm");
                    responseString = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удается получить версию обновления.");
                }
            }
            return responseString;
        }

        private static void DirectoryCopy(
        string sourceDirName, string destDirName, bool copySubDirs, List<string> excludeFiles = null)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files.Where(x=> excludeFiles != null && !excludeFiles.Contains(x.Name)))
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs.Where(x=>!destDirName.Contains(x.FullName)))
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }

    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}

