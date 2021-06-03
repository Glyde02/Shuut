using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Shuut
{
    class TCP_Client : TCP_Connection
    {
        static string ipAdress = "127.0.0.1";
        static int port = 433;
        private Socket socket;
        private World world;
        private Camera camera;
        private ProgressBar progress;
        private TextBlock text;

        byte[] inputData = new byte[4*8];

        public TCP_Client(string ip, ProgressBar progress, TextBlock text)
        {
            ipAdress = ip;
            this.progress = progress;
            this.text = text;
        }

        public override byte[] Get()
        {
            while (true)
            {
                StringBuilder inputMessage = new StringBuilder();
                int bytesRead = 0;

                bytesRead = socket.Receive(inputData);

                world.map[(int)world.pX * world.width + (int)world.pY] = 0;
                double[] data = this.Decrypt(inputData);
                if (data[3] != 0)
                {
                    camera.NewLocation(world);
                }
                world.pX = data[0];
                world.pY = data[1];
                world.angle = data[2];
                world.map[(int)world.pX * world.width + (int)world.pY] = 2;
            }
        }

        public override void Send(byte[] message)
        {
            socket.Send(message);
        }

        public override void Init(World world, Camera camera)
        {
            this.world = world;
            this.camera = camera;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipPoint);
            text.Dispatcher.BeginInvoke(new Action(() => text.Text = "Сonnection succeed"));
            progress.Dispatcher.BeginInvoke(new Action(() => progress.Visibility = System.Windows.Visibility.Hidden));
            
            
            Task.Run(() => Get());
        }

        public void CloseConnection()
        {
            socket.Close();
        }

    }
}
