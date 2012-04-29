using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mandelbrot
{
    class Program
    {
        public static int WIDTH = 1000;
        public static int HEIGHT = 1000;
        public static int MAX_ITER = 400;

        static void Main(string[] args)
        {


            Stopwatch watch = new Stopwatch();

            watch.Start();

            Console.WriteLine("Generating...");
            
            MandelbrotGenerator gen = new MandelbrotGenerator();
            byte[] bitmap = gen.generate(WIDTH, HEIGHT, MAX_ITER);

            Bitmap bmp = CopyDataToBitmap(bitmap, WIDTH, HEIGHT);
            bmp.Save("./truc.png", ImageFormat.Png);
            watch.Stop();
            Console.WriteLine("Done in {0} ms!", watch.ElapsedMilliseconds);
            Console.ReadKey();
        }


        public static Bitmap CopyDataToBitmap(byte[] data, int x, int y)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(x, y, PixelFormat.Format24bppRgb);

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);


            //Unlock the pixels
            bmp.UnlockBits(bmpData);


            //Return the bitmap 
            return bmp;
        }
    }


}
