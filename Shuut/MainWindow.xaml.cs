using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SFML;
using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using SFML.Window;
using System.Threading;

namespace Shuut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window 
    {
        private SFML.Graphics.RenderWindow window;
        private World world = new World();
        private Camera camera = new Camera();
        private Map map = new Map();
        private Weapon weapon = new Weapon();
        private TCP_Connection connection;
        private Dictionary<Keyboard.Key, bool> keysArePressed = new Dictionary<Keyboard.Key, bool>
        {
           {Keyboard.Key.A, false},
           {Keyboard.Key.S, false},
           {Keyboard.Key.D, false},
           {Keyboard.Key.W, false},
           {Keyboard.Key.Space, false}
        };
        private bool isFocus = true;


        public MainWindow()
        {
            InitializeComponent();



        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {




            //var sound = new Sound(GenerateSineWave(frequency: 440.0, volume: .25, seconds: 1));
            ContextSettings settings = new ContextSettings();

            settings.AntialiasingLevel = 8;

            
            


            window = new RenderWindow(new SFML.Window.VideoMode((uint)world.windowWidth, (uint)world.windowHeight), "SFML running in .NET Core", Styles.Default, settings);

            window.Closed += (_, __) => window.Close();
            //window.KeyPressed += new EventHandler<SFML.Window.KeyEventArgs>(Keyboard);
            //window.KeyPressed += OnKeyPressed;
            window.KeyReleased += OnKeyReleased;
            window.LostFocus += LostFocus_;
            window.GainedFocus += GainedFocus_;
            

            //sound.Play();

            bool isShot = false;
            //if (connection is TCP_Server)
            //{
            //    (connection as TCP_Server).window = window;
            //}


            while (window.IsOpen)
            {
                window.DispatchEvents();


                if (isFocus)
                {
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
                    if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Space) &&
                        keysArePressed[Keyboard.Key.Space] == false)
                    {
                        keysArePressed[Keyboard.Key.Space] = true;
                        if (camera.CheckShot(world))
                        {
                            isShot = true;
                        }
                    }
                }


                if (!(bool)checkSingle.IsChecked)
                {

                }
                else
                {
                    if (!isShot)
                        connection.Send(connection.Encrypt(new double[4] { camera.pX, camera.pY, camera.angle, 0 }));
                    else
                        connection.Send(connection.Encrypt(new double[4] { camera.pX, camera.pY, camera.angle, 1 }));
                }
                isShot = false;

                

                window.Clear(Color.Black);
                camera.View(window, world);

                map.ShowMap(world, camera, window);
                weapon.ShowWeapon(window);

                window.Display();
            }
        }

        private void GainedFocus_(object sender, EventArgs e)
        {
            isFocus = true;
        }

        private void LostFocus_(object sender, EventArgs e)
        {
            isFocus = false;
        }

        public void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            if (isFocus)
            {
                if (e.Code == Keyboard.Key.Space && !keysArePressed[Keyboard.Key.Space])
                {
                    keysArePressed[SFML.Window.Keyboard.Key.A] = true;
                    //do
                }                
            }

        }
        public void OnKeyReleased(object sender, SFML.Window.KeyEventArgs e)
        {
            if (isFocus)
            {
                if (e.Code == Keyboard.Key.Space)
                {
                    keysArePressed[Keyboard.Key.Space] = false;
                }
            }

        }


        private void btnGenerateMap_Click(object sender, RoutedEventArgs e)
        {
            MapGenerator newMap = new MapGenerator();
            newMap.Generate(100, 100);
        }

        private void btnLoadMap_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                world.GetMap("test.txt", 100, 100);
                btnPlay.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show("Map loading error!");
            }
        }

        private void btnServer_Click(object sender, RoutedEventArgs e)
        {
            connection = new TCP_Server();
            connection.Init(world, camera);
            
        }
        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            connection = new TCP_Client();
            connection.Init(world, camera);
            camera.pX = 20;
            camera.angle = Math.PI;
            //connection.Send(connection.Encrypt(new double[4] { 20, 20, 0, 0 }));
        }

        private void btnLoadTexture_Click(object sender, RoutedEventArgs e)
        {



            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                //this will call in background thread
                showElement(prgBarTexture);
                camera.LoadTexture();
                hideElement(prgBarTexture);
                showElement(txtTexture);
                Thread.Sleep(3000);
                hideElement(txtTexture);

            });

            

        }

        private void hideElement(UIElement obj)
        {
            this.Dispatcher.Invoke((Action)(() => {
                obj.Visibility = Visibility.Hidden;
            }));
        }
        private void showElement(UIElement obj)
        {
            this.Dispatcher.Invoke((Action)(() => {
                obj.Visibility = Visibility.Visible;
            }));
        }
    }

}
