using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class SyncSockClient
    {
        static int port;
        static string ipAddr;

        public static void StartClient(string GameInput)
        {
            //Data buffer for income data
            byte[] bytes = new byte[1024];


            //Try to connect to a remote device
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = IPAddress.Parse(ipAddr);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                //Create a TCP/IP socket
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                //Connect the socket to the remote endpoint
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Connected to {0}", sender.RemoteEndPoint.ToString());

                    //Encode the data string into a byte array
                    byte[] msg = Encoding.ASCII.GetBytes(GameInput + "<EOF>");

                    //Send the data through the socket
                    int bytesSent = sender.Send(msg);

                    //Receive The response from the remote device
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    //Release the Socket
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane);
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected Exception : {0}", ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static int Main(string[] args)
        {
            string input = "";

            Console.WriteLine("Type in the Server's IPV6 address...");
            ipAddr = Console.ReadLine();


            Console.WriteLine("Type in the port you want use to connect...");
            input = Console.ReadLine();
            port = Convert.ToInt32(input);

            do
            {
                //Pass in Method from game-play input
                StartClient(GamePlayInput.Choice());
                Console.WriteLine("Would you like to play again? Type Y to continue playing");
                input = Console.ReadLine();

            } while (input == "y" || input =="Y");

            Console.ReadKey();
            return 0;

        }
    }
}
