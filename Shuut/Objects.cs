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

        public RectangleShape Pixel(int x, int y, Color color)
        {
            RectangleShape pixel = new RectangleShape()
            {
                Size = new Vector2f(3, 3),
                Position = new Vector2f(x, y),
                FillColor = color
            };
            return pixel;
        }

    }
}
