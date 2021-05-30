using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Shuut
{
    class Camera
    {
        public int pX = 4, pY = 5;
        private double angle = 5;
        private double FOV = Math.PI / 3;
        public int deph = 30;
        private double raySpeed = 0.1;




        public void Round(RenderWindow window, World world)
        {

            

            //var line = new RectangleShape(new Vector2f(1, 15));
            //line.Position = new Vector2f(0, 0);


            //window.Draw(line);


            //var secline = new RectangleShape(new Vector2f(1, 30));
            //secline.Position = new Vector2f(30, 0);

            //window.Draw(secline);


            for (int x = 0; x < world.windowWidth; x++)
            {
                double rayAngle = angle + FOV / 2 - (x * FOV / world.windowWidth);
                double rayX = Math.Cos(rayAngle);
                double rayY = Math.Sin(rayAngle);

                double dist = 0;
                bool hit = false;

                while (!hit && dist < deph)
                {
                    dist += raySpeed;

                    int tx = (int)(pX + rayX * dist);
                    int ty = (int)(pY + rayY * dist);

                    if (tx < 0 || tx >= deph + pX || ty < 0 || ty >= deph + pY)
                    {
                        hit = true;
                        dist = deph;
                    }
                    else
                    {
                        if (world.map[tx, ty] == 1)
                        {
                            hit = true;
                        }
                    }


                }

                //int wall = (int)(world.windowHeight / 2 * (1 - 1 / dist));
                //int floor = (int)(world.windowHeight / 2 * (1 + 1 / dist));

                int wall = (int)(world.windowHeight / 2d - world.windowHeight * FOV / dist);
                int floor = world.windowHeight - wall;

                var line = new RectangleShape(new Vector2f(1, floor - wall));
                line.Position = new Vector2f(x, wall);


                window.Draw(line);

                //window.SetTitle(angle.ToString());





            }

            //angle++;
            //if (angle >= 360)
            //{
            //    angle = 0;
            //}


        }

        public void Left(World world)
        {
            pX -= (int)(Math.Sin(Math.PI - angle) * 2);

            pY -= (int)(Math.Cos(Math.PI - angle) * 2);

            if (pX < 0 || pY < 0 || world.map[pX, pY] == 1)
            {
                pX += (int)(Math.Sin(Math.PI - angle) * 2);

                pY += (int)(Math.Cos(Math.PI - angle) * 2);
            }
        }
        public void Right(World world)
        {
            pX += (int)(Math.Sin(Math.PI - angle) * 2);

            pY += (int)(Math.Cos(Math.PI - angle) * 2);

            if (pX < 0 || pY < 0 || world.map[pX, pY] == 1)
            {
                pX -= (int)(Math.Sin(Math.PI - angle) * 2);

                pY -= (int)(Math.Cos(Math.PI - angle) * 2);
            }

            //if (world.map[pX, pY] == 1)
            //{
            //    pY += (int)(Math.Sin(angle) * 2);

            //    pX += (int)(Math.Cos(angle) * 2);
            //}
        }
        public void Forward(World world)
        {
            pX += (int)(Math.Cos(angle)*2);

            pY += (int)(Math.Sin(angle)*2);

            if (pX < 0 || pY < 0 || world.map[pX, pY] == 1)
            {
                pX -= (int)(Math.Cos(angle)*2);

                pY -= (int)(Math.Sin(angle)*2);
            }

            //pY++;
        }
        public void Backward(World world)
        {
            pX -= (int)(Math.Cos(angle) * 2);

            pY -= (int)(Math.Sin(angle) * 2);

            if (pX < 0 || pY < 0 || world.map[pX, pY] == 1)
            {
                pX += (int)(Math.Cos(angle) * 2);

                pY += (int)(Math.Sin(angle) * 2);
            }
        }
        public void IncAngle()
        {
            angle += 0.1;
        }
        public void DecAngle()
        {
            angle -= 0.1;
        }



    }




}
