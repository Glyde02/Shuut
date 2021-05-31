using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SFML;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;

namespace Shuut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window 
    {
        private World world = new World();
        private Camera camera = new Camera();
        private Map map = new Map();
        private Weapon weapon = new Weapon();
        private TCP_Connection connection;

        public MainWindow()
        {
            InitializeComponent();



        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {




            //camera.Round(world);




            //var shape = new RectangleShape(new Vector2f(100, 100))
            //{
            //    FillColor = Color.Black
            //};

            //var sound = new Sound(GenerateSineWave(frequency: 440.0, volume: .25, seconds: 1));
            ContextSettings settings = new ContextSettings();

            settings.AntialiasingLevel = 8;

            //sf::RenderWindow window(sf::VideoMode(800, 600), "SFML shapes", sf::Style::Default, settings);
            



            var window = new RenderWindow(new SFML.Window.VideoMode((uint)world.windowWidth, (uint)world.windowHeight), "SFML running in .NET Core", Styles.Default, settings);

            window.Closed += (_, __) => window.Close();

            //sound.Play();


            ////window.Draw(shape);

            bool isPressed = false;
            if (connection is TCP_Server)
            {
                (connection as TCP_Server).window = window;
            }

            while (window.IsOpen)
            {
                window.DispatchEvents();

                if (connection is TCP_Server)
                {

                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.A))
                    {
                        isPressed = true;
                        camera.Left(world);

                    }
                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.D))
                    {
                        isPressed = true;
                        camera.Right(world);
                    }
                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.W))
                    {
                        isPressed = true;
                        camera.Forward(world);
                    }
                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.S))
                    {
                        isPressed = true;
                        camera.Backward(world);
                    }
                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.N))
                    {
                        isPressed = true;
                        camera.IncAngle();
                    }
                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.M))
                    {
                        isPressed = true;
                        camera.DecAngle();
                    }
                }
                else
                    isPressed = true;
                if (isPressed)
                {
                    window.SetTitle(camera.pX.ToString() + ":" + camera.pY.ToString());
                    connection.Send(connection.Encrypt(new double[4] { camera.pX, camera.pY, camera.angle, 0 }));
                    isPressed = false;
                    
                }
                

                window.Clear(Color.Black);
                camera.Round(window, world);
                //map.ShowMap(world, camera, window);

                weapon.ShowWeapon(window);

                //window.Clear(Color.Black);
                ////window.Draw(shape);
                //camera.Round(window, world);
                window.Display();
                //System.Threading.Thread.Sleep(50);
            }
        }

        private void btnGenerateMap_Click(object sender, RoutedEventArgs e)
        {
            MapGenerator newMap = new MapGenerator();
            newMap.Generate(200, 200);
        }

        private void btnLoadMap_Click(object sender, RoutedEventArgs e)
        {
            world.GetMap("test.txt");
        }

        private void btnServer_Click(object sender, RoutedEventArgs e)
        {
            connection = new TCP_Server();
            connection.Init(world);
            
        }
        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            connection = new TCP_Client();
            connection.Init(world);
            camera.pX = 20;
            camera.angle = Math.PI;
            //connection.Send(connection.Encrypt(new double[4] { 20, 20, 0, 0 }));
        }





        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            connection.Send(connection.Encrypt(new double[4] { 2.3, 5, 6, 7.89}));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double[] str = connection.Decrypt(connection.Get());
            t1.Text = str[0].ToString();
            t1.Text = str[1].ToString();
            t1.Text = str[2].ToString();
            t1.Text = str[3].ToString();
        }
    }

    


}
