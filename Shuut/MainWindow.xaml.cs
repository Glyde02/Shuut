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
        private Sound step;
        private Sound shot;
        private Vector2f mouse_dot;
        private Dictionary<Keyboard.Key, bool> keysArePressed = new Dictionary<Keyboard.Key, bool>
        {
           {Keyboard.Key.A, false},
           {Keyboard.Key.S, false},
           {Keyboard.Key.D, false},
           {Keyboard.Key.W, false},
           {Keyboard.Key.Space, false}
        };
        
        private bool isFocus = true;
        private bool loadMap = false;
        private bool loadTexture = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContextSettings settings = new ContextSettings();
            settings.AntialiasingLevel = 8;
            window = new RenderWindow(new SFML.Window.VideoMode((uint)world.windowWidth, (uint)world.windowHeight), "Shuut", Styles.Default, settings);
            window.Closed += (_, __) => window.Close();
            window.KeyReleased += OnKeyReleased;
            window.LostFocus += LostFocus_;
            window.GainedFocus += GainedFocus_;          

            bool isShot = false;
            bool firsAnimation = false;

            Clock clock = new Clock();
            int loop = 0;
            bool isStep = false;

            double num = 0;


            while (window.IsOpen)
            {
                int tm = clock.ElapsedTime.AsMilliseconds();
                loop += tm;
                clock.Restart();

                window.DispatchEvents();


                if (isFocus)
                {
                    if (pressedLeft())
                    {
                        isStep = true;
                        camera.Left(world);
                    }
                    if (pressedRight())
                    {
                        isStep = true;
                        camera.Right(world);
                    }
                    if (pressedForward())
                    {
                        isStep = true;
                        camera.Forward(world);
                    }
                    if (pressedBackward())
                    {
                        isStep = true;
                        camera.Backward(world);
                    }
                    if (pressedInc())
                    {
                        camera.IncAngle();
                    }
                    if (pressedDec())
                    {
                        camera.DecAngle();
                    }
                    if (pressedShot() &&
                        keysArePressed[Keyboard.Key.Space] == false)
                    {
                        weapon.ShotPlay();
                        keysArePressed[Keyboard.Key.Space] = true;
                        firsAnimation = true;
                        if (camera.CheckShot(world, ref num))
                        {
                            isShot = true;
                        }
                    }

                    if (isStep && loop > 600)
                    {
                        camera.StepPlay();
                        loop = 0;
                    }
                }


                if (!(bool)checkSingle.IsChecked)
                {

                }
                else
                {
                    if (!isShot)
                        connection.Send(connection.Encrypt(new double[6] { camera.pX, camera.pY, camera.angle, 0, camera.number, 0 }));
                    else
                        connection.Send(connection.Encrypt(new double[6] { camera.pX, camera.pY, camera.angle, 1, camera.number, num}));
                }
                isShot = false;

                //Joystick
                //if (Joystick.Count > 0)
                //    window.SetTitle("This!");
                //window.SetTitle(Joystick.GetAxisPosition(1, Joystick.Axis.X).ToString());


                window.Clear(Color.Black);
                camera.View(window, world);

                if (keysArePressed[Keyboard.Key.Space] == true && firsAnimation)
                {
                    weapon.ShowFire(window);
                    firsAnimation = false;
                }

                map.ShowMap(world, camera, window);
                weapon.ShowWeapon(window);

                window.Display();
                isStep = false;
            }
        }

        private bool pressedLeft()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.A))
                return true;
            else
                return false;
        }
        private bool pressedRight()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.D))
                return true;
            else
                return false;
        }
        private bool pressedForward()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.W))
                return true;
            else
                return false;
        }
        private bool pressedBackward()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.S))
                return true;
            else
                return false;
        }
        private bool pressedInc()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.N))
                return true;
            else
                return false;
        }
        private bool pressedDec()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.M))
                return true;
            else
                return false;
        }
        private bool pressedShot()
        {
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Space))
                return true;
            else
                return false;
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
                world.GetMap("map.mp", 100, 100);
                loadMap = true;
                ButtonPressed();
            }
            catch
            {
                MessageBox.Show("Map loading error!");
            }
        }

        private void btnServer_Click(object sender, RoutedEventArgs e)
        {
            txtIpClient.Visibility = Visibility.Hidden;
            prgBarServer.Visibility = Visibility.Visible;
            cmbIp.Items.Clear();
            cmbIp.Visibility = Visibility.Visible;
            connection = new TCP_Server(prgBarServer, txtConnectInfo);
            foreach(string ip in (connection as TCP_Server).GetIp())
            {
                cmbIp.Items.Add(ip);
            }
            
            
        }
        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                prgBarClient.Visibility = Visibility.Visible;
                connection = new TCP_Client(txtIpClient.Text, prgBarClient, txtConnectInfo);
                connection.Init(world, camera);
                camera.pX = 20;
                camera.angle = Math.PI;
            }
            catch
            {
                MessageBox.Show("Error IP!");
            }
        }

        private void btnLoadTexture_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                showElement(prgBarTexture);
                camera.LoadTexture();
                hideElement(prgBarTexture);
                showElement(txtTexture);
                Thread.Sleep(3000);
                hideElement(txtTexture);

            });
            loadTexture = true;
            ButtonPressed();

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

        private void cmbIp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (connection as TCP_Server).ipAdress = cmbIp.SelectedItem.ToString();
            connection.Init(world, camera);
        }

        public void ButtonPressed()
        {
            if (!(bool)checkSingle.IsChecked)
            {
                if (loadMap && loadTexture)
                    btnPlay.IsEnabled = true;
            }
            else
                if (loadMap && loadTexture && txtConnectInfo.Text != "")
                    btnPlay.IsEnabled = true;

        }

        private void prgBarServer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (prgBarServer.Visibility == Visibility.Hidden)
                ButtonPressed();
        }

        private void prgBarClient_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (prgBarClient.Visibility == Visibility.Hidden)
                ButtonPressed();
        }
    }

}
