using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Shuut
{
    class Weapon
    {
        public void ShowWeapon(RenderWindow window)
        {
            RectangleShape horz = new RectangleShape(new SFML.System.Vector2f(20, 2))
            {
                FillColor = Color.Red,
                Position = new SFML.System.Vector2f(window.Size.X / 2 - 10, window.Size.Y / 2 - 1)
            };
            RectangleShape vert = new RectangleShape(new SFML.System.Vector2f(2, 20))
            {
                FillColor = Color.Red,
                Position = new SFML.System.Vector2f(window.Size.X / 2 - 1, window.Size.Y / 2 - 10)
            };
            window.Draw(horz);
            window.Draw(vert);



            Texture gun = new Texture("gun_ok.png");
            


            Sprite weapon = new Sprite();
            weapon.Texture = gun;

            weapon.Position = new SFML.System.Vector2f(400, 245);
            
            window.Draw(weapon);
        }

    }
}
