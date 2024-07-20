using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServerExample
{
    class Program
    {
        static ASCIIEncoding encoding = new ASCIIEncoding();
        static void Main(string[] args)
        {
            int port = 10000;
            IPAddress ipAddress1 = IPAddress.Parse("127.0.0.1");
            TcpListener server1 = new TcpListener(ipAddress1, port);

            IPAddress ipAddress2 = IPAddress.Parse("127.0.0.2");
            TcpListener server2 = new TcpListener(ipAddress2, port);

            server1.Start();
            server2.Start();
            Console.WriteLine("Đang lắng nghe kết nối sever 1...");
            Console.WriteLine("Đang lắng nghe kết nối sever 2...");

            while (true)
            {
                TcpClient client1 = server1.AcceptTcpClient();
                Console.WriteLine("Đã kết nối với client sever 1...");
                Thread clientThread1 = new Thread(HandleClientServer1);
                clientThread1.Start(client1);

                TcpClient client2 = server2.AcceptTcpClient();
                Console.WriteLine("Đã kết nối với client sever 2...");
                Thread clientThread2 = new Thread(HandleClientServer2);
                clientThread2.Start(client2);
            }
        }

        private static void HandleClientServer1(object obj)
        {
            TcpClient client = (TcpClient)obj;
            try
            {
                NetworkStream stream = client.GetStream();
                while (true)
                {
                    string signal = "*[Reader][1]123456789[!]";
                    byte[] data = encoding.GetBytes(signal);
                    stream.Write(data);
                    Console.WriteLine($"Đã gửi sever 1: {signal}");
                    Thread.Sleep(3000); 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi: {e.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        private static void HandleClientServer2(object obj)
        {
            TcpClient client = (TcpClient)obj;
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    for (int i = 20000; i < 20100; i +=10)
                    {
                        string signal = $"*[Reader][{i}]{DateTime.Now:dd/MM/yyyy HH:mm:ss}[!]";
                        byte[] data = encoding.GetBytes(signal);
                        stream.Write(data);
                        Console.WriteLine($"Đã gửi sever 2: {signal}");
                        Thread.Sleep(300);
                    }

                    for (int i = 1; i < 50; i++)
                    {
                        string signal = $"*[Reader][20000]{DateTime.Now:dd/MM/yyyy HH:mm:ss}[!]";
                        byte[] data = encoding.GetBytes(signal);
                        stream.Write(data);
                        Console.WriteLine($"Đã gửi sever 2: {signal}");
                        Thread.Sleep(300);
                    }

                    for (int i = 1; i < 6; i++)
                    {
                        string s = $"*[Reader][10]{DateTime.Now:dd/MM/yyyy HH:mm:ss}[!]";
                        byte[] d = encoding.GetBytes(s);
                        stream.Write(d);
                        Console.WriteLine($"Đã gửi sever 2: {s}");
                        Thread.Sleep(300);
                    }

                    Thread.Sleep(100000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Lỗi: {e.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
