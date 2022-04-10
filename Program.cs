using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using System;

namespace DuckView
{
    internal class Program
    {
        const string INPUT_PATH = @"\\192.168.1.180\sdcard\games\PSX\";
        const string OUTPUT_PATH = @"C:\Users\Eric\Downloads\";
        const int width = 500;

        static void Main(string[] args)
        {
            Console.WriteLine("STARTING SCAN");

            ScanDirectory(INPUT_PATH);
        }

        static void ScanDirectory(string parentPath)
        {
            foreach (string filePath in Directory.GetFiles(parentPath))
            {
                string ext = Path.GetExtension(filePath);
                if (ext == ".duckmap")
                {
                    ProcessMap(filePath);
                }
            }

            foreach (string childPath in Directory.GetDirectories(parentPath))
            {
                string filename = Path.GetFileName(childPath);
                if (filename == "#DUCK")
                    continue;

                ScanDirectory(childPath);
            }
        }

        static void ProcessMap(string mapPath)
        {
            Console.WriteLine($"Reading Map: {mapPath}");

            byte[] map = File.ReadAllBytes(mapPath);

            Console.WriteLine(" - building Image");

            int pixels = map.Length * 8;
            int height = pixels / width;
            if (pixels % width != 0)
                height++;

            using (Image<Rgba32> image = new Image<Rgba32>(width, height))
            {
                byte one = (byte)1;
                for (int pixel = 0; pixel < pixels; pixel++)
                {
                    int x = pixel % width;
                    int y = pixel / width;
                    int index = pixel / 8;
                    byte bit = (byte)(pixel % 8);
                    bool set = ((map[index] >> bit) & one) != 0;

                    image[x, y] = set ? Color.Red : Color.White;
                }

                Console.WriteLine(" - writing Image");

                string outputFilename = Path.GetFileName(Path.ChangeExtension(mapPath, ".png"));
                string outputPath = Path.Combine(OUTPUT_PATH, outputFilename);
                image.SaveAsPng(outputPath);
            }

            Console.WriteLine("DONE");

        }
    }
}