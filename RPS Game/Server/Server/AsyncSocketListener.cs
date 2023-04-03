using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class StateObject
    {
        //Size of receive buffer
        public const int BufferSize = 1024;

        //Receive buffer
        public byte[] Buffer = new byte[BufferSize];

        //Received data string
        public StringBuilder sb = new StringBuilder();

        //Client Socket
        public Socket? Clisocket = null;
    }

    public class AysncSocketListener
    {
        //Thread signal
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        //Chosen port to host on
        public static int port;

        public static void StartListening()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress,port);

            //Create TCP/IP socket
            Socket listener = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //Bind the socket to the local endpoint and listen for income connections
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while(true)
                {
                    //Set the event to non-signaled state
                    allDone.Reset();

                    //Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Hosting on IP: {0} and Port: {1}\nWaiting for a connection...",iPAddress, port);
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    //Wait until a connection is made before continuing
                    allDone.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            //Signal the main thread to continue.
            allDone.Set();

            //Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            //Creates the state object
            StateObject state = new StateObject();
            state.Clisocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            string content = String.Empty;

            //Retrieve the state object and the handler socket from the Async state object
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.Clisocket;

            //Read data from the client socket
            int bytesRead = handler.EndReceive(ar);

            if(bytesRead > 0)
            {
                //There might be ore data, so store the data received so far
                state.sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

                //Check for end-of-file tag. If it is not there read for more data
                content = state.sb.ToString();
                
                if(content.IndexOf("<EOF>") > -1)
                {
                    //All the data has been read from the file client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \nData: {1}", content.Length, content);

                    //Echo the data back to the client
                    Send(handler, RockPaperScissorsAI.CliWinRPS(content));
                }
                else
                {
                    //Not all data received. Get more
                    handler.BeginReceive(state.Buffer,0,StateObject.BufferSize,0,new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            //Convert the string to byte data using ASCII encoding
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            //Begin sending the data to the remote device
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback),handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                //Retrieve the socket from the state object
                Socket handler = (Socket) ar.AsyncState;

                //Complete sending the data to the remote device
                int BytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client", BytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static int Main(string[] args)
        {
            string input ="";

            Console.WriteLine("Type in a port to host on...");
            input = Console.ReadLine();
            port = Convert.ToInt32(input);

            StartListening();
            return 0; 
        }
    }
}