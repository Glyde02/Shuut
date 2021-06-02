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
using System.Windows;

namespace Shuut
{
    class MapGenerator
    {
        public List<RectangleShape> pixels = new List<RectangleShape>();

        public void Generate(int w, int h)
        {
            Objects elem = new Objects();

            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode((uint)w, (uint)h), "");
            window.Closed += (_, __) => window.Close();


            pixels.Clear();
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

            if (MessageBox.Show("Save map?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                byte[] map = new byte[w * h];
                foreach (RectangleShape dot in pixels)
                {
                    int index = (int)(dot.Position.X * (w) + dot.Position.Y);
                    map[index] = 1;
                }

                for (int i = 0; i < w; i++)
                {
                    map[0 * 0 + i] = 1;
                    map[h * (w - 1) + i] = 1;
                }
                for (int j = 0; j < h; j++)
                {
                    map[j * w + 0] = 1;
                    map[j * w + (w - 1)] = 1;
                }

                FileStream F = new FileStream("map.mp", FileMode.OpenOrCreate,
                            FileAccess.ReadWrite);

                F.Write(map, 0, w * h);
                F.Close();
            }

        }


    }
}
