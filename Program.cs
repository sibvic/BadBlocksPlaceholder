using System;
using System.IO;

namespace BadBlocksPlaceholder
{
    class Program
    {
        static void Main(string[] args)
        {
            string targetDir;
            if (args[0] == "clean")
            {
                if (!args[1].ToUpper().Contains("BADBLOCKPLACEHOLDERS"))
                {
                    Console.WriteLine("Only BadBlockPlaceholders folder is allowed to be cleaned");
                    return;
                }
                targetDir = args[1];
            }
            else
            {
                var drive = new DriveInfo(args[0]);
                int blockSize = int.Parse(args[1]) * 1024;
                targetDir = CreateBlocks(drive, blockSize);
            }

            Validate(targetDir);
            Console.WriteLine("Done!");
        }

        private static string CreateBlocks(DriveInfo drive, int blockSize)
        {
            var block = new byte[blockSize];
            var bbDir = Path.Combine(drive.RootDirectory.FullName, "BadBlockPlaceholders");
            if (!Directory.Exists(bbDir))
                Directory.CreateDirectory(bbDir);

            var targetDir = Path.Combine(bbDir, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            int index = 0;
            while (drive.AvailableFreeSpace > blockSize)
            {
                Console.WriteLine("Creating block #" + index);
                var filename = Path.Combine(targetDir, index + ".bad");
                ++index;
                using (var filestream = File.OpenWrite(filename))
                {
                    filestream.Write(block, 0, blockSize);
                    filestream.Flush();
                    filestream.Close();
                }
            }
            return targetDir;
        }

        private static void Validate(string targetDir)
        {
            Console.WriteLine("Validating blocks");
            foreach (var file in Directory.GetFiles(targetDir))
            {
                try
                {
                    var res = File.ReadAllBytes(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                File.Delete(file);
            }
        }
    }
}
