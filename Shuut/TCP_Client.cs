using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shuut
{
    class TCP_Client : TCP_Connection
    {
        static string ipAdress = "127.0.0.1";
        static int port = 433;
        private Socket socket;
        private World world;

        byte[] inputData = new byte[4*8];

        public override byte[] Get()
        {
            while (true)
            {
                StringBuilder inputMessage = new StringBuilder();
                int bytesRead = 0;

                bytesRead = socket.Receive(inputData);

                world.map[(int)world.pX * world.width + (int)world.pY] = 0;
                double[] data = this.Decrypt(inputData);
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

        public override void Init(World world)
        {
            this.world = world;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipAdress), port);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipPoint);

            Task.Run(() => Get());
        }

        public void CloseConnection()
        {
            socket.Close();
        }
    }
}
