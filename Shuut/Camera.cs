using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Shuut
{
    class Camera
    {
        public double pX = 10, pY = 10;
        public double angle = 0;
        private double FOV = Math.PI / 3;
        public int deph = 200;
        private double raySpeed = 0.15;
        private int numTexture = 1;
        Texture[] textu_wall;
        Texture[] textu_sky;
        Image img_wall = new Image("wall3.jpg");
        Image img_sky = new Image("sky2.png");
        Texture pll = new Texture("player2.png");
        Sound step;

        public Camera()
        {
            SoundBuffer buffer1 = new SoundBuffer("step.wav");
            step = new Sound(buffer1);
        }

        public void LoadTexture()
        {
            textu_wall = new Texture[1000];
            for (int i = 0; i < 1000; i++)
            {
                textu_wall[i] = new Texture(img_wall, new IntRect(i, 0, 1, 800));
            }

            textu_sky = new Texture[6000];
            for (int i = 0; i < 6000; i++)
            {
                textu_sky[i] = new Texture(img_sky, new IntRect(i, 0, 1, 600));
            }
        }


        public void View(RenderWindow window, World world)
        {
            numTexture = 0;

            angle = angle % (Math.PI * 2);


            double numSky = (int)(-angle * 180 / Math.PI * (world.windowWidth / 60d));
            if (numSky < 0)
                numSky = 6000 + numSky;

            var sand = new RectangleShape(new Vector2f(world.windowWidth, world.windowHeight/2))
            {
                Position = new Vector2f(0, world.windowHeight / 2),
                FillColor = new Color(134, 117, 105)
            };
            window.Draw(sand);


            for (int x = 0; x < world.windowWidth; x++)
            {
                //Sky
                var skyLine = new RectangleShape(new Vector2f(1, world.windowHeight / 2))
                {
                    Position = new Vector2f(x, 0),
                    Texture = textu_sky[(int)numSky]
                };
                numSky++;
                if (numSky >= 6000)
                {
                    numSky = 0;
                }
                window.Draw(skyLine);




                double rayAngle = angle + FOV / 2 - (x * FOV / world.windowWidth);
                double rayX = Math.Cos(rayAngle);
                double rayY = Math.Sin(rayAngle);

                double dist = 0;
                bool hit = false;
                bool isBound = false;
                bool player = false;

                double tx;
                double ty;


                while (!hit && dist < deph)
                {
                    dist += raySpeed;

                    tx = (pX + rayX * dist);
                    ty = (pY + rayY * dist);

                    if (tx < 0 || tx >= deph + pX || ty < 0 || ty >= deph + pY || tx > world.width || ty > world.height)
                    {
                        hit = true;
                        dist = deph;
                    }
                    else
                    {
                        if (world.map[(int)((int)tx * world.width + (int)ty)] == 2)
                        {
                            hit = true;
                            player = true;


                        }
                        else
                        {
                            if (world.map[(int)((int)tx * world.width + (int)ty)] == 1)
                            {
                                hit = true;

                                var boundsVectorsList = new List<(double X, double Y)>();

                                for (int cx = 0; cx < 2; cx++)
                                {
                                    for (int cy = 0; cy < 2; cy++)
                                    {
                                        double vx = (int)tx + cx - pX;
                                        double vy = (int)ty + cy - pY;

                                        double vectorModule = Math.Sqrt(vx * vx + vy * vy);
                                        double cosAngle = (rayX * vx / vectorModule) + (rayY * vy / vectorModule);
                                        boundsVectorsList.Add((vectorModule, cosAngle));
                                    }
                                }

                                boundsVectorsList = boundsVectorsList.OrderBy(v => v.X).ToList();

                                double boundAngle = 0.03 / dist;

                                if (Math.Acos(boundsVectorsList[0].Y) < boundAngle ||
                                    Math.Acos(boundsVectorsList[1].Y) < boundAngle)
                                    isBound = true;

                            }
                        }


                    }


                }

                //int wall = (int)(world.windowHeight / 2 * (1 - 1 / dist));
                //int floor = (int)(world.windowHeight / 2 * (1 + 1 / dist));

                if (dist < deph)
                {

                    //int wall = (int)(world.windowHeight / 2d - world.windowHeight * FOV / dist);
                    //int floor = world.windowHeight - wall;


                    int wall = (int)((world.windowHeight / 2) * (1 - 3 / dist));
                    int floor = (int)((world.windowHeight / 2) * (1 + 3 / dist));

                    if (player)
                    {
                        var Player = new RectangleShape(new Vector2f(1, floor - wall))
                        {
                            Position = new Vector2f(x, wall),
                            Texture = pll
                        };

                        window.Draw(Player);
                        player = false;
                    }
                    else
                    {

                        if (isBound)
                        {
                            numTexture = 1;
                        }
                        if (numTexture >= 1000)
                        {
                            numTexture = 1;
                        }


                        var line = new RectangleShape(new Vector2f(1, floor - wall))
                        {
                            Position = new Vector2f(x, wall),

                            //Without texture. White --> Black
                            //FillColor = !isBound ? new Color(255, 255, 255, (byte)(255 - (255 * dist / deph))) :
                            //                    new Color(255, 255, 255, (byte)(255 * dist / deph)),

                            Texture = textu_wall[numTexture]
                        };
                        window.Draw(line);





                        numTexture += (int)((dist) / 2);
                        //numTexture++;
                        isBound = false;
                    }

                    var fog = new RectangleShape(new Vector2f(1, floor - wall))
                    {
                        Position = new Vector2f(x, wall),
                        FillColor = new Color(255, 255, 255, (byte)(230 * dist / deph))
                    };
                    window.Draw(fog);




                }
            }
        }

        public void Left(World world)
        {
            pX -= (Math.Sin(Math.PI - angle) * 0.5);

            pY -= (Math.Cos(Math.PI - angle) * 0.5);

            if (pX < 0 || pY < 0 || world.map[(int)((int)pX * world.width + (int)pY)] == 1)
            {
                pX += (Math.Sin(Math.PI - angle) * 0.5);

                pY += (Math.Cos(Math.PI - angle) * 0.5);
            }
        }
        public void Right(World world)
        {
            pX += (Math.Sin(Math.PI - angle) * 0.5);

            pY += (Math.Cos(Math.PI - angle) * 0.5);

            if (pX < 0 || pY < 0 || world.map[(int)((int)pX * world.width + (int)pY)] == 1)
            {
                pX -= (Math.Sin(Math.PI - angle) * 0.5);

                pY -= (Math.Cos(Math.PI - angle) * 0.5);
            }
        }
        public void Forward(World world)
        {
            pX += (Math.Cos(angle) * 0.5);

            pY += (Math.Sin(angle) * 0.5);

            if (pX < 0 || pY < 0 || world.map[(int)((int)pX * world.width + (int)pY)] == 1)
            {
                pX -= (Math.Cos(angle) * 0.5);

                pY -= (Math.Sin(angle) * 0.5);
            }
        }
        public void Backward(World world)
        {
            pX -= (Math.Cos(angle) * 0.5);

            pY -= (Math.Sin(angle) * 0.5);

            if (pX < 0 || pY < 0 || world.map[(int)((int)pX * world.width + (int)pY)] == 1)
            {
                pX += (Math.Cos(angle) * 0.5);

                pY += (Math.Sin(angle) * 0.5);
            }
        }
        public void IncAngle()
        {
            angle += 0.07;
            if (angle >= Math.PI * 2)
            {
                angle = 0;
            }
        }
        public void DecAngle()
        {
            angle -= 0.07;
            if (angle <= -Math.PI * 2)
            {
                angle = 0;
            }
        }

        public bool CheckShot(World world)
        {
            //double rayAngle = angle + FOV / 2 - ((world.windowWidth / 2) * FOV / world.windowWidth);
            double rayAngle = angle;
            double rayX = Math.Cos(rayAngle);
            double rayY = Math.Sin(rayAngle);

            double dist = 0;
            bool hit = false;

            double tx;
            double ty;


            while (!hit && dist < deph)
            {
                dist += raySpeed;

                tx = (pX + rayX * dist);
                ty = (pY + rayY * dist);


                if (world.map[(int)((int)tx * world.width + (int)ty)] == 1)
                {
                    dist = deph;
                }
                else if (world.map[(int)((int)tx * world.width + (int)ty)] == 2)
                {
                    hit = true;
                }

            }
            return hit ? true : false;
        }

        public void StepPlay()
        {
            step.Play();
        }

        public void NewLocation(World world)
        {
            Random rand = new Random();

            while (true)
            {
                pX = rand.Next(2, 98);
                pY = rand.Next(2, 98);
                if (world.map[(int)((int)pX * world.width + (int)pY)] != 1)
                    return;
            }

            //pX += 1;
            //pY += 1;
        }



    }




}
