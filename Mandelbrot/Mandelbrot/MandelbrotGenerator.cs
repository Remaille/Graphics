using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Drawing;
using System.Threading.Tasks;

namespace Mandelbrot
{
    class MandelbrotGenerator
    {

        Color[] Colors = new Color[512];

        private void generatePalette()
        {
            Parallel.For(0, 512, delegate(int i)
            {
                Colors[i] = Color.FromArgb(Math.Min(255, i*10), Math.Min(255, i*3), Math.Min(255,i));

            });
        }


        public byte[] generate(int width, int height, int maxIter)
        {
            generatePalette();

            byte[] bitmap = new byte[width * height * 3];

            Complex min = new Complex(-2, -1.5);
            Complex max = new Complex(1.0, min.Imaginary + (1 - min.Real) * height / width);

            Parallel.For(0, width, delegate(int x)
            {
                Parallel.For(0, height, delegate(int y)
                {
                    Complex c = new Complex(min.Real + x * (max.Real - min.Real) / (width - 1), max.Imaginary - y * (max.Imaginary - min.Imaginary) / (height - 1));
                    Complex z = new Complex(c.Real, c.Imaginary);
                    bool insideSet = true;
                    int currentIter = 1;

                    for (int n = 0; n < maxIter; n++)
                    {
                        if (z.Magnitude > 4)
                        {
                            insideSet = false;
                            break;
                        }
                        else
                        {
                            z = z * z + c;
                            currentIter++;
                        }
                    }

                    if (!insideSet)
                    {
                        float nSmooth = (float)(currentIter + 1 - Math.Log(Math.Log(z.Magnitude)) / Math.Log(2));
                        Color col = GetColor(nSmooth, maxIter);
                        bitmap[(y + x * height) * 3] = col.B;
                        bitmap[(y + x * height) * 3 + 1] = col.G;
                        bitmap[(y + x * height) * 3 + 2] = col.R;
                    }
                });
           });

            //for (int x = 0; x < width; x++)
            //{
            //    for (int y = 0; y < height; y++)
            //    {
            //        Complex c = new Complex(min.Real + x * (max.Real - min.Real) / (width - 1), max.Imaginary - y * (max.Imaginary - min.Imaginary) / (height - 1));
            //        Complex z = new Complex(c.Real, c.Imaginary);
            //        bool insideSet = true;
            //        int currentIter = 1;

            //        for (int n = 0; n < maxIter; n++)
            //        {
            //            if (z.Magnitude > 4)
            //            {
            //                insideSet = false;
            //                break;
            //            }
            //            else
            //            {
            //                z = z * z + c;
            //                currentIter++;
            //            }
            //        }
                    
            //        if (!insideSet)
            //        {
            //            float nSmooth = (float)(currentIter + 1 - Math.Log(Math.Log(z.Magnitude)) / Math.Log(2));
            //            Color col = GetColor(nSmooth, maxIter);
            //            bitmap[(y + x * height) * 3] = col.B;
            //            bitmap[(y + x * height) * 3 + 1] = col.G;
            //            bitmap[(y + x * height) * 3 + 2] = col.R;
            //        }
            //    }
            //}
            return bitmap;
        }

        private Color GetColor(double mu, int maxIter)
        {
            mu = mu / maxIter * Colors.Length;
            int clr1 = (int)mu % Colors.Length;
            int clr2 = (clr1 + 1) % Colors.Length;
            double t2 = mu - clr1;
            double t1 = 1 - t2;
            byte r = (byte)(Colors[clr1].R * t1 + Colors[clr2].R * t2);
            byte g = (byte)(Colors[clr1].G * t1 + Colors[clr2].G * t2);
            byte b = (byte)(Colors[clr1].B * t1 + Colors[clr2].B * t2);
            return Color.FromArgb(255, r, g, b);
        }
    }
}
