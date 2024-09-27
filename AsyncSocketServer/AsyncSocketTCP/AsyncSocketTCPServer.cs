using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocketTCP
{
    public class AsyncSocketTCPServer
    {
        //Khai báo các thuộc tính của lớp
        IPAddress mIP;
        int mPort;
        TcpListener mTCPListener;
        List<TcpClient> mClients;  
        
        //Lấy trạng thái của chương trình
        public bool KeepRunning { get; set; }

        //Phương thức tạo lập
        public AsyncSocketTCPServer()
        {
            mClients = new List<TcpClient>();//Tạo một list để chứa thông tin của Client
        }
        //Phương thức lắng nghe, nhận kết nối từ client, thêm vào danh sách
        public async void StartListeningForIcomingConnection(IPAddress ipaddr=null,int port=9001)
        {
            if(ipaddr==null)
            {
                ipaddr = IPAddress.Any;//Nếu không có địa chỉ ip thì gán cho một địa chỉ bất kì
            }    
            if(port <= 0)//Nếu giá trị của port nhập vào dưới 0 thì gán lại giá trị cho port bằng 9001
            {
                port = 9001;
            }
            //Truyền giá trị của đối số nhập vào cho mIP và mPort
            mIP = ipaddr;
            mPort = port;

            //In thông tin ra cửa sổ Output
            Debug.WriteLine(string.Format("IP address: {0} - Port: {1}",mIP.ToString(),mPort));


            //Tạo một đối tượng mới của lớp TCPListener có 2 tham số là mIP(IPAddress) và mPort(cổng port)
            mTCPListener =new TcpListener(mIP, mPort);


            //Chạy cái lệnh trong khối try, nếu có lỗi thì sẽ được chuyển sang khối catch
            try
            {
                mTCPListener.Start();//Bắt đầu lắng nghe kết nối
                KeepRunning = true;//gán giá trị keepRunning = true, true ở đây có nghĩa là chương trình đang thực thi
                while(KeepRunning) 
                {
                    var returnedByAccept = await mTCPListener.AcceptTcpClientAsync();//Chấp nhận kết nối một cách bất đồng bộ
                    //await khiến cho chương trình chờ cho tới khi dòng lệnh chứa nó thực hiện xong
                    //AcceptTcpClientAsync() là một phương thức bất đồng bộ của TcpListener

                    mClients.Add(returnedByAccept);//Thêm kết nối mới vào danh sách

                    //Xuất ra thông tin của địa chỉ kết nối thành công
                    Debug.WriteLine(String.Format("Client connected successfully, count: {0} - {1}",mClients.Count,returnedByAccept.Client.RemoteEndPoint));
                    //Debug.WriteLine:In thông báo ra cửa sổ Ôutput trong chế độ Debug
                    //mClents.Count: Trả về số phần tử kết nối đang hoạt động trong mClients (Số lượng kết nối trong hoạt động)
                    //returnedByAccept.Client.RemoteEndPoint: Trả về địa chỉ IP và cổng của địa chỉ đang được kết nối

                    TakeCareOfTCPClient(returnedByAccept);//Gọi phương thức TakeCareOfTCPClient và truyền đối số returnedByAccept vào
                }
            }
            catch(Exception excp)
            {
                Debug.WriteLine(excp.ToString());//Xuất ra thông tin về ngoại lệ được ném từ khối try
            }
        }
        //Phương thức xóa client
        private void RemoveClient(TcpClient paramClient)
        {
            if(mClients.Contains(paramClient))
            {
                mClients.Remove(paramClient);
                Debug.WriteLine(String.Format("Client removed, count: {0}", mClients.Count));
            }    
            //Xóa client đang kết nối khỏi danh sách
        }
        //Phương thức quản lý các client, nhận tin nhắn từ client
        private async void TakeCareOfTCPClient(TcpClient paramClient)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream= paramClient.GetStream();
                reader = new StreamReader(stream);

                char[] buff= new char[64];//tạo mảng buff chứa được 64 ký tự

                while (KeepRunning)
                {
                    Debug.WriteLine("*** Ready to read");
                    int nRet = await reader.ReadAsync(buff, 0, buff.Length);
                    Debug.WriteLine("Returned: " + nRet);

                    if(nRet == 0)
                    {
                        RemoveClient(paramClient);
                        Debug.WriteLine("Socket disconnected");
                        break;
                    }
                    string receivedText = new string(buff);
                    Debug.WriteLine("*** RECEIVED: " + receivedText);
                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch(Exception excp)
            {
                RemoveClient(paramClient);//Xóa thiết bị đang hoạt động
                Debug.WriteLine(excp.ToString());//Xuất ngoại lệ ra cửa sổ output
            }
        }

        //Phương thức gửi tin nhắn đến tất cả các Client
        public async void SendToAll(string leMessage)
        {
            if (string.IsNullOrEmpty(leMessage))
                return;
            try 
            {
                byte[] buffMessage = Encoding.ASCII.GetBytes(leMessage);
                foreach(TcpClient c in mClients)
                {
                    await c.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
                }
            }
            catch (Exception excp)
            {
                Debug.WriteLine(excp.ToString() );
            }
        }

        //Phương thức ngắt kết nối tất cả client và dừng server
        public void StopServer()
        {
            try
            {
                if (mTCPListener != null)
                    mTCPListener.Stop();
                foreach (TcpClient c in mClients)
                    c.Close();
                mClients.Clear(); 
            }
            catch (Exception excp)
            {
                Debug.WriteLine(excp.ToString());
            }
        }

        
    }
}
