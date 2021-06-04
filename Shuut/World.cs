using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shuut
{
    struct Player
    {
        public double num;
        public double pX;
        public double pY;
    };

    class World
    {
        //public byte[] map = new byte[] {    1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
        //                                    1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,
        //                                    1,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,
        //                                    1,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,1,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
        //                                    1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 };



        public int width, height;
        public byte[] map;
        public List<Player> players = new List<Player>();
        public int windowWidth = 1000, windowHeight = 800;

        public double pX = 15, pY=10, angle=0;


        public void GetMap(string name, int w, int h)
        {
            this.width = w;
            this.height = h;
            this.map = new byte[w * h];
            try
            {
                FileStream F = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);
                F.Read(map, 0, w * h);
                F.Close();
                //map[(int)pX * width + (int)pY] = 2;

            }
            catch
            {
                MessageBox.Show("Map loading error!");
            }

        }
        public double CheckCoordinate(double tx, double ty)
        {
            foreach(Player player in players)
            {
                if ((int)player.pX == (int)tx && (int)player.pY == (int)ty)
                {
                    return player.num;
                }
            }
            return -1;
        }

        public void ClearPlayer(double num)
        {
            bool isYes = false;

            foreach (Player player in players)
            {
                if (player.num == num)
                {
                    this.map[(int)player.pX * this.width + (int)player.pY] = 0;
                    isYes = true;
                }
            }
            if (!isYes)
            {
                Player pl = new Player();
                pl.pX = 0;
                pl.pY = 0;
                pl.num = num;
                players.Add(pl);
            }
        }
        public void SetNewData(double pX, double pY, double angle, double num)
        {
            Player pl = new Player();
            pl.pX = pX;
            pl.pY = pY;
            pl.num = num;
            this.map[(int)pX * this.width + (int)pY] = 2;


            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].num == num)
                {
                    players.RemoveAt(i);
                    players.Add(pl);
                    break;
                }
            }
            
        }

    }
}
