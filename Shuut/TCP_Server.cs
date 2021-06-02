using SFML.Window;
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
    class TCP_Server : TCP_Connection
    {
        private World world;
        private Camera camera;

        private Socket socket;
        public string ipAdress = "127.0.0.1";
        private int port = 433;

        private List<Socket> listSockets = new List<Socket>();
        private Socket clientSocket;
        public Window window;
        public ProgressBar progress;
        private TextBlock text;

        byte[] inputData = new byte[8 * 4];

        public TCP_Server(ProgressBar progress, TextBlock text)
        {
            this.progress = progress;
            this.text = text;
        }

        public override byte[] Get()
        {
            while (true)
            {
                StringBuilder inputMessage = new StringBuilder();
                int bytesRead = 0;
                

                bytesRead = clientSocket.Receive(inputData);

                if (inputData != null)
                {
                    world.map[(int)world.pX * world.width + (int)world.pY] = 0;
                    double[] data = this.Decrypt(inputData);
                    if (data[3] != 0)
                    {
                        camera.NewLocation(world);
                    }
                    world.pX = data[0];
                    world.pY = data[1];
                    world.angle = data[2];
                    world.map[(int)(world.pX) * world.width + (int)world.pY] = 2;
                    //SendMessageToAll(clientSocket);
                }
                
            }
        }

        public void SendMessageToAll(Socket clientSocket)
        {
            foreach (Socket someSoket in listSockets)
            {
                if (clientSocket != someSoket)
                {
                    someSoket.Send(inputData);
                }
            }
        }

        public override void Init(World world, Camera camera)
        {
            this.world = world;
            this.camera = camera;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            this.socket.Listen(5);

            Task.Run(lstn);
        }

        private void lstn()
        {
            while (true)
            {
                clientSocket = this.socket.Accept();
                this.listSockets.Add(clientSocket);
                text.Dispatcher.BeginInvoke(new Action(() => text.Text = "Сonnection succeed"));
                progress.Dispatcher.BeginInvoke(new Action(() => progress.Visibility = System.Windows.Visibility.Hidden));
                
                this.isConnected = true;


                Task.Run(() => Get());
            }
        }

        public override void Send(byte[] message)
        {
            foreach (Socket sock in listSockets)
            {
                sock.Send(message);
            }
        }
        public List<string> GetIp()
        {
            List<string> adresses = new List<string>();

            String host = System.Net.Dns.GetHostName();
            // Получение ip-адреса.
            //ipAdress = Dns.GetHostByName(host).AddressList[0].ToString();

            
            int i = 0;
            while (i < Dns.GetHostEntry(host).AddressList.Length)
            {
                if (Dns.GetHostEntry(host).AddressList[i].AddressFamily.ToString() == "InterNetwork")
                {
                    ipAdress = Dns.GetHostEntry(host).AddressList[i].ToString();
                    adresses.Add(ipAdress);
                }
                i++;
            }

            return adresses;
        }
    }
}
