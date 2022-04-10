using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.ColorSpaces;
using System;

namespace DuckView
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = @"\\192.168.1.180\sdcard\games\PSX\Castlevania - Symphony of the Night (U)\Castlevania - Symphony of the Night (U).bin.duckmap";
            string output = @"C:\Users\Eric\Downloads\map.png";
            int width = 500;

            Console.WriteLine("Reading Map");

            byte[] map = File.ReadAllBytes(input);

            Console.WriteLine("Building Image");

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

                Console.WriteLine("Writing Image");

                image.SaveAsPng(output);
            }

            Console.WriteLine("DONE");
        }
    }
}