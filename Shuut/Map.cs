using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shuut
{
    class Map
    {
        private int size = 2;
        private int width = 125, height = 60;


        public void ShowMap(World world, Camera camera, RenderWindow window)
        {
            RectangleShape map = new RectangleShape(new SFML.System.Vector2f(world.width, world.height))
            {
                FillColor = Color.White
            };
            window.Draw(map);


            //CircleShape circle = new CircleShape(camera.deph)
            //{
            //    Position = new SFML.System.Vector2f(camera.pY * size - camera.deph / size, camera.pX * size - camera.deph / size ),
            //    OutlineColor = Color.Green,
            //    OutlineThickness = 1

            //};
            //window.Draw(circle);

            RectangleShape player = new RectangleShape(new SFML.System.Vector2f(5, 5))
            {
                FillColor = Color.Red,
                Position = new SFML.System.Vector2f((float)(camera.pY), (float)(camera.pX))
            };
            window.Draw(player);

            for (int i = 0; i < world.height; i++)
                for (int j = 0; j < world.width; j++)
                {
                    if (world.map[i * world.width + j] == 1)
                    {
                        RectangleShape dot = new RectangleShape(new SFML.System.Vector2f(size, size))
                        {
                            FillColor = Color.Blue,
                            Position = new SFML.System.Vector2f(j, i)
                        };
                        window.Draw(dot);
                    }
                }




        }



    }
}
