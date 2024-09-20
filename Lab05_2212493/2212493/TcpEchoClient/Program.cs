using System; // For Int32, ArgumentException
using System.Text; // For Encoding
using System.IO; // For IOException
using System.Net; // For IPAddress
using System.Net.Sockets; // For TcpListener, Socket
class TcpEchoClient
{
    static void Main(string[] args)
    {
        if ((args.Length < 2) || (args.Length > 3))
        {
            throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
        }
        String server = args[0];
        byte[] data = Encoding.ASCII.GetBytes(args[1]);
        //Use port number if present, otherwise default to 9000
        int port = (args.Length == 3) ? Int32.Parse(args[2]) : 9000;
        TcpClient client = null;
        NetworkStream ns = null;
        try
        {
            client = new TcpClient(server, port);
            Console.WriteLine("Conneted to server... sending echo string");
            ns = client.GetStream();
            ns.Write(data, 0, data.Length);
            int totalBytesRcv = 0;
            int bytesRcv = 0;
            while (totalBytesRcv < data.Length)
            {
                if ((bytesRcv = ns.Read(data, totalBytesRcv,
                    data.Length - totalBytesRcv)) == 0)
                {
                    Console.WriteLine("Lost connection");
                    break;
                }
                totalBytesRcv += bytesRcv;
            }
            Console.WriteLine("Received {0} bytes from server: {1}", totalBytesRcv,
                Encoding.ASCII.GetString(data, 0, totalBytesRcv));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            ns.Close();
            client.Close();
        }
    }
}