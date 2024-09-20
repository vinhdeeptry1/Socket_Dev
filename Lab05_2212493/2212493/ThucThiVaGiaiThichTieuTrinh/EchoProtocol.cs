using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThucThiVaGiaiThichTieuTrinh
{
    internal class EchoProtocol : IProtocol
    {
        public const int BUFFERSIZE = 32;
        private Socket clntSock;
        private ILogger logger;

        public EchoProtocol(Socket clntSock, ILogger logger)
        {
            this.clntSock = clntSock;
            this.logger = logger;
        }

        public void handleclient()
        {
            ArrayList entry = new ArrayList();
            entry.Add("Client Address and Port: " + clntSock.RemoteEndPoint);
            entry.Add("Thread= " + Thread.CurrentThread.GetHashCode());
            try
            {
                int recvMsgSize;
                int totalByteEchoed = 0;
                byte[] rcvBuffer = new byte[BUFFERSIZE];
                try
                {
                    while ((recvMsgSize =  clntSock.Receive(rcvBuffer, 0, rcvBuffer.Length, SocketFlags.None)) > 0) 
                    {
                        clntSock.Send(rcvBuffer, 0, recvMsgSize, SocketFlags.None);
                        totalByteEchoed += recvMsgSize;
                    }
                }
                catch (SocketException se) 
                {
                    entry.Add(se.ErrorCode + ": " + se.Message);
                }
                entry.Add("Client finished; echoed " + totalByteEchoed + "byte.");
            }
            catch (SocketException se)
            {
                entry.Add(se.ErrorCode + ": " + se.Message);
            }
            clntSock.Close();
            logger.writeEntry(entry);
        }
    }
}
