using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoLPatcherProxy
{
    public class SimpleProxy
    {
        public Socket MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<Tuple<Socket, Socket>> Sockets = new List<Tuple<Socket, Socket>>();
        public Task ClientTask, ServerTask;

        public static byte[] GetFakeResponse(string body)
        {
            string temp = "HTTP/1.1 200 OK\r\nServer: Apache\r\nAccept-Ranges: bytes\r\nContent-Type: text/plain\r\nConnection: keep-alive\r\nPragma: no-cache\r\nCache-Control: max-age=0, no-cache, no-store\r\n";
            int bytes = body.Length;
            byte[] buffer;
            temp += "Content-Length: " + bytes + "\r\n\r\n";
            temp += body;
            buffer = Encoding.UTF8.GetBytes(temp);
            return buffer;

        }

        public static byte[] GetFakeResponse(byte[] bytes)
        {
            string temp = "HTTP/1.1 200 OK\r\nServer: Apache\r\nAccept-Ranges: bytes\r\nContent-Type: text/plain\r\nConnection: keep-alive\r\nPragma: no-cache\r\nCache-Control: max-age=0, no-cache, no-store\r\n";
            temp += "Content-Length: " + bytes.LongLength + "\r\n\r\n";
            byte[] headers = Encoding.UTF8.GetBytes(temp);
            byte[] output = new byte[headers.LongLength + bytes.LongLength];
            System.Buffer.BlockCopy(headers, 0, output, 0, headers.Length);
            System.Buffer.BlockCopy(bytes, 0, output, headers.Length, bytes.Length);
            return output;
        }
        public static byte[] GetFakeResponseHeaders(Stream s)
        {
            string temp = "HTTP/1.1 200 OK\r\nServer: Apache\r\nAccept-Ranges: bytes\r\nContent-Type: text/plain\r\nConnection: keep-alive\r\nPragma: no-cache\r\nCache-Control: max-age=0, no-cache, no-store\r\n";
            temp += "Content-Length: " + s.Length + "\r\n\r\n";
            byte[] headers = Encoding.UTF8.GetBytes(temp);
            return headers;
        }

        private static byte[] Replace(byte[] input, byte[] pattern, byte[] replacement)
        {
            if (pattern.Length == 0)
            {
                return input;
            }

            List<byte> result = new List<byte>();

            int i;

            for (i = 0; i <= input.Length - pattern.Length; i++)
            {
                bool foundMatch = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (input[i + j] != pattern[j])
                    {
                        foundMatch = false;
                        break;
                    }
                }

                if (foundMatch)
                {
                    result.AddRange(replacement);
                    i += pattern.Length;
                    break;//found one match, I'M DONE
                }
                else
                {
                    result.Add(input[i]);
                }
            }

            for (; i < input.Length; i++)
            {
                result.Add(input[i]);
            }

            return result.ToArray();
        }

        private void ClientToServerTask(int id)
        {
            bool error = false;
            byte[] buffer = new byte[1024];
            byte[] toSend, body, headers;
            int bytesRead = 0;
            string data, type = "", path = "";
            bool requestCompleted = false;

            Socket clientSocket = Sockets[id].Item1;
            Socket serverSocket = Sockets[id].Item2;

            NetworkStream net = new NetworkStream(clientSocket); //Need socket to send huge files!

            while (!error)
            {
                try
                {
                    requestCompleted = false;
                    bytesRead = clientSocket.Receive(buffer, 1024, SocketFlags.None);
                    if (bytesRead == 0)
                        throw new Exception();
                    buffer = Replace(buffer, Program.localhost, Program.riothost);
                    bytesRead += Program.diff;
                    data = Encoding.UTF8.GetString(buffer, 0, 4);
                    if (data == "GET ")
                    {
                        type = "GET";
                        data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        path = data.Split(' ')[1];

                        if (File.Exists("httpd" + path))
                        {
                            type = "OVERRIDEN GET";
                            using (FileStream fs = File.OpenRead("httpd" + path))
                            {
                                headers = GetFakeResponseHeaders(fs);
                                net.Write(headers, 0, headers.Length);
                                fs.CopyTo(net);
                                net.Flush();
                            }
                            //body = body.Replace("\r\n", "\n");
                            //body = body.Replace("\n", "\r\n");
                            //clientSocket.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, null, null);//send back the fake request to client,
                            requestCompleted = true;//without sending anything to server
                        }

                    }

                    if (!requestCompleted)
                        serverSocket.BeginSend(buffer, 0, bytesRead, SocketFlags.None, null, null);
                    Console.WriteLine("[{2}][{0}] {1}", type, path, id);
                }
                catch { error = true; }
            }
            try
            {
                clientSocket.Close();
            }
            catch { }

            try
            {
                serverSocket.Close();
            }
            catch { }
            Console.WriteLine("Client task #{0} ended", id);
        }

        private void ServerToClientTask(int id)
        {
            bool error = false;
            byte[] buffer = new byte[1024*64];
            int bytesRead = 0;
            Socket clientSocket = Sockets[id].Item1;
            Socket serverSocket = Sockets[id].Item2;

            while (!error)
            {
                try
                {
                    bytesRead = serverSocket.Receive(buffer, 1024*64, SocketFlags.None);
                    if (bytesRead == 0)
                        throw new Exception();
                    if (clientSocket != null)
                        clientSocket.BeginSend(buffer, 0, bytesRead, SocketFlags.None, null, null);
                }
                catch { error = true; }
            }
            try
            {
                serverSocket.Close();
            }
            catch { }

            try
            {
                clientSocket.Close();
            }
            catch { }

            Console.WriteLine("Server task #{0} ended", id);
        }


        public SimpleProxy(EndPoint local, string remote, int remotePort)
        {
            MainSocket.Bind(local);
            MainSocket.Listen(10);
            while (true)
            {
                try
                {
                    int id = Sockets.Count;
                    Socket ClientSocket = MainSocket.Accept();

                    Console.WriteLine("Connection from " + ClientSocket.RemoteEndPoint.ToString());
                    Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ServerSocket.Connect(remote, remotePort);
                    Console.WriteLine("Connection to server established.");

                    ClientSocket.NoDelay = true;
                    Sockets.Add(new Tuple<Socket, Socket>(ClientSocket, ServerSocket));
                    Task.Factory.StartNew(() => { ServerToClientTask(id); });
                    Task.Factory.StartNew(() => { ClientToServerTask(id); });

                }
                catch { Console.WriteLine("ERROR while setting up"); }
            }
        }
    }
}
