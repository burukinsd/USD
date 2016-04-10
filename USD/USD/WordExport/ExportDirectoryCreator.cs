using System.IO;

namespace USD.WordExport
{
    public static class ExportDirectoryCreator
    {
        public static string EnsureDirectory()
        {
            var direcName = DirectoryHelper.GetDataDirectory() + "��� �������� ������";
            var direc = new DirectoryInfo(direcName);
            if (!direc.Exists)
            {
                direc.Create();
            }
            return direc.FullName;
        }
    }
}