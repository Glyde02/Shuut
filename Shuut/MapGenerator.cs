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

            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode((uint)800, (uint)600), "Map generator");
            window.Closed += (_, __) => window.Close();

            while (window.IsOpen)
            {
                window.DispatchEvents();


                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    pixels.Add(elem.Pixel(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y, Color.Green));
                }

                foreach (RectangleShape dot in pixels)
                {
                    window.Draw(dot);
                }
                

                //window.Draw(dots, PrimitiveType.Quads);
                window.Display();


            }

            byte[] map = new byte[800 * 600];
            foreach (RectangleShape dot in pixels)
            {
                map[(int)(dot.Position.X * 800 + dot.Position.Y)] = 1;
            }

            FileStream F = new FileStream("test.txt", FileMode.OpenOrCreate,
                        FileAccess.ReadWrite);

            F.Write(map, 0, 800 * 600);
            F.Close();



        }

    }
}
