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
            


            while (window.IsOpen)
            {
                window.DispatchEvents();


                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.A))
                {
                    camera.Left(world);
                }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.D))
                {
                    camera.Right(world);
                }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.W))
                {
                    camera.Forward(world);
                }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.S))
                {
                    camera.Backward(world);
                }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.N))
                {
                    camera.IncAngle();
                }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.M))
                {
                    camera.DecAngle();
                }



                window.Clear(Color.Black);
                camera.Round(window, world);
                //map.ShowMap(world, camera, window);


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
    }

    


}
