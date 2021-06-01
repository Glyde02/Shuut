using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shuut
{
    abstract class TCP_Connection
    {
        abstract public void Init(World world, Camera camera);

        abstract public void Send(byte[] message);

        abstract public byte[] Get();

        public double[] Decrypt(byte[] input)
        {

            double[] result = new double[4];
            int buffInd = 0;

            for (int i = 0; i < 4; i++)
            {
                byte[] buffnumb = new byte[8];
                for (int k = 0; k < 8; k++)
                {
                    buffnumb[k] = input[k + buffInd];
                }
                result[i] = BitConverter.ToDouble(buffnumb, 0);
                buffInd += 8;
            }

            return result;

        }

        public byte[] Encrypt(double[] input)
        {

            byte[] result = new byte[4*8];
            int resultInd = 0;

            foreach (double i in input)
            {
                byte[] buff = BitConverter.GetBytes(i);
                foreach (byte k in buff)
                {
                    result[resultInd] = k;
                    resultInd++;
                }
            }
            return result;

        }

    }
}
