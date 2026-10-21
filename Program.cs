using System;
using System.IO;

namespace BadBlocksPlaceholder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintHelp();
                return;
            }

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

        private static void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  BadBlocksPlaceholder <drive> <block size in KB>");
            Console.WriteLine("      Fills all free space on the drive with files of the specified size, then reads them.");
            Console.WriteLine("      Files that read without errors are deleted, others are left in <drive>\\BadBlockPlaceholders\\yyyyMMdd.");
            Console.WriteLine("      Example: BadBlocksPlaceholder e:\\ 1024");
            Console.WriteLine();
            Console.WriteLine("  BadBlocksPlaceholder clean <BadBlockPlaceholders folder>");
            Console.WriteLine("      Re-tests files in the folder and deletes the ones without reading errors.");
            Console.WriteLine("      Example: BadBlocksPlaceholder clean e:\\BadBlockPlaceholders\\20190110");
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
