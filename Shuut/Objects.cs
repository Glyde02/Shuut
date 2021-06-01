using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Shuut
{
    class Objects
    {

        public RectangleShape Pixel(int x, int y, int size, Color color)
        {
            RectangleShape pixel = new RectangleShape()
            {
                Size = new Vector2f(size, size),
                Position = new Vector2f(x, y),
                FillColor = color
            };
            return pixel;
        }

    }
}
