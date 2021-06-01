using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.System;
using System.IO;

namespace Shuut
{
    class MapGenerator
    {
        public List<RectangleShape> pixels = new List<RectangleShape>();

        public void Generate(int w, int h)
        {
            Objects elem = new Objects();

            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode((uint)w, (uint)h), "Map generator");
            window.Closed += (_, __) => window.Close();



            while (window.IsOpen)
            {
                window.DispatchEvents();


                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    pixels.Add(elem.Pixel(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y, 5, Color.Green));
                }

                foreach (RectangleShape dot in pixels)
                {
                    window.Draw(dot);
                }
                

                //window.Draw(dots, PrimitiveType.Quads);
                window.Display();


            }



            byte[] map = new byte[w * h];
            foreach (RectangleShape dot in pixels)
            {
                int index = (int)(dot.Position.X * (w) + dot.Position.Y);
                map[index] = 1;
                //map[index + 1] = 1;
                //map[index + 2] = 1;
                //index += w;
                //map[index] = 1;
                //map[index + 1] = 1;
                //map[index + 2] = 1;
                //index += w;
                //map[index] = 1;
                //map[index + 1] = 1;
                //map[index + 2] = 1;
            }

            for (int i = 0; i < w; i++)
            {
                map[0 * 0 + i] = 1;
                map[h * (w-1) + i] = 1;
            }
            for (int j = 0; j < h; j++)
            {
                map[j * w + 0] = 1;
                map[j * w + (w-1)] = 1;
            }


            //for (int j = 0; j < h; j++)
            //    {

            //        map[]
            //        pixels.Add(elem.Pixel(i, 0, 1, Color.Green));
            //        pixels.Add(elem.Pixel(i, h - 1, 1, Color.Green));
            //        pixels.Add(elem.Pixel(0, j, 1, Color.Green));
            //        pixels.Add(elem.Pixel(w - 1, j, 1, Color.Green));
            //    }

            FileStream F = new FileStream("test.txt", FileMode.OpenOrCreate,
                        FileAccess.ReadWrite);

            F.Write(map, 0, w * h);
            F.Close();



        }

    }
}
