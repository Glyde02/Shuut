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



namespace Shuut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }


        private static SoundBuffer GenerateSineWave(double frequency, double volume, int seconds)
        {
            uint sampleRate = 44100;
            var samples = new short[seconds * sampleRate];

            for (int i = 0; i < samples.Length; i++)
                samples[i] = (short)(Math.Sin(frequency * (2 * Math.PI) * i / sampleRate) * volume * short.MaxValue);

            return new SoundBuffer(samples, 1, sampleRate);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            World world = new World();
            Camera camera = new Camera();
            Map map = new Map();
            

            //camera.Round(world);




            //var shape = new RectangleShape(new Vector2f(100, 100))
            //{
            //    FillColor = Color.Black
            //};

            //var sound = new Sound(GenerateSineWave(frequency: 440.0, volume: .25, seconds: 1));

            var window = new RenderWindow(new SFML.Window.VideoMode((uint)world.windowWidth, (uint)world.windowHeight), "SFML running in .NET Core");

            window.Closed += (_, __) => window.Close();

            //sound.Play();


            ////window.Draw(shape);
            


            while (window.IsOpen)
            {
                window.DispatchEvents();


                if (Keyboard.IsKeyDown(Key.A))
                {
                    camera.Left(world);
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    camera.Right(world);
                }
                if (Keyboard.IsKeyDown(Key.W))
                {
                    camera.Forward(world);
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    camera.Backward(world);
                }
                if (Keyboard.IsKeyDown(Key.N))
                {
                    camera.IncAngle();
                }
                if (Keyboard.IsKeyDown(Key.M))
                {
                    camera.DecAngle();
                }



                window.Clear(Color.Black);
                camera.Round(window, world);
                map.ShowMap(world, camera, window);


                //window.Clear(Color.Black);
                ////window.Draw(shape);
                //camera.Round(window, world);
                window.Display();
                System.Threading.Thread.Sleep(50);
            }
        }
    }

    


}
