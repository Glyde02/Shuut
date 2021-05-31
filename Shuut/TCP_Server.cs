using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shuut
{
    class TCP_Server : TCP_Connection
    {
        private World world;

        private Socket socket;
        private string ipAdress = "127.0.0.1";
        private int port = 433;

        private List<Socket> listSockets = new List<Socket>();
        private Socket clientSocket;
        public Window window;

        byte[] inputData;


        public override byte[] Get()
        {
            while (true)
            {
                StringBuilder inputMessage = new StringBuilder();
                int bytesRead = 0;
                inputData = new byte[8 * 4];

                bytesRead = clientSocket.Receive(inputData);

                if (inputData != null)
                {
                    world.map[(int)world.pX * world.width + (int)world.pY] = 0;
                    double[] data = this.Decrypt(inputData);
                    world.pX = data[0];
                    world.pY = data[1];
                    world.angle = data[2];
                    world.map[(int)(world.pX) * world.width + (int)world.pY] = 2;
                    window.SetTitle(world.pX.ToString() + ":" + world.pY.ToString());
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

        public override void Init(World world)
        {
            this.world = world;

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
    }
}
