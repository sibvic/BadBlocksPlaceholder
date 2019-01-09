using System;
using System.IO;

namespace BadBlocksPlaceholder
{
    class Program
    {
        static void Main(string[] args)
        {
            var drive = new DriveInfo(args[0]);
            int blockSize = int.Parse(args[1]) * 1024;
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
            Console.WriteLine("Done!");
        }
    }
}
