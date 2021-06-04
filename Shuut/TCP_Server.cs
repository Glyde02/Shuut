using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        //private Socket clientSocket;
        public ProgressBar progress;
        private TextBlock text;

        private double number = 1;

        //byte[] inputData = new byte[8 * 6];

        public TCP_Server(ProgressBar progress, TextBlock text)
        {
            this.progress = progress;
            this.text = text;
        }

        public override byte[] Get()
        {
            return new byte[1];
        }

        public byte[] Get(Socket client)
        {
            while (true)
            {
                int bytesRead = 0;

                byte[] inputData = new byte[6 * 8];
                bytesRead = client.Receive(inputData);

                if (inputData != null)
                {
                    double[] data = this.Decrypt(inputData);
                    if (camera.number != data[4])
                    {
                        world.ClearPlayer(data[4]);
                        world.SetNewData(data[0], data[1], data[2], data[4]);
                    }

                    if (data[3] != 0 && camera.number == data[5])
                    {
                        camera.NewLocation(world);
                    }

                    SendMessageToAll(client, inputData);

                    //SendMessageToAll(clientSocket);
                }
                
            }
        }

        public void SendMessageToAll(Socket clientSocket, byte[] input)
        {
            foreach (Socket someSoket in listSockets)
            {
                if (clientSocket != someSoket)
                {
                    someSoket.Send(input);
                }
            }
        }

        public override void Init(World world, Camera camera)
        {
            this.world = world;
            this.camera = camera;
            camera.number = 0;

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
                Socket clientSocket = this.socket.Accept();
                this.listSockets.Add(clientSocket);
                clientSocket.Send(Encrypt(new double[6] { 0, 0, 0, 0, 0, number }));

                if (number > 1)
                {
                    byte[] inputData = Encrypt(new double[6] { 0, 0, 0, 0, number, 0 });
                    SendMessageToAll(clientSocket, inputData);
                }

                world.ClearPlayer(number);
                number++;
                text.Dispatcher.BeginInvoke(new Action(() => text.Text = "Сonnection succeed"));
                progress.Dispatcher.BeginInvoke(new Action(() => progress.Visibility = System.Windows.Visibility.Hidden));
                
                this.isConnected = true;

                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    Get(clientSocket);

                });
                //Task.Run(() => Get(clientSocket));
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
