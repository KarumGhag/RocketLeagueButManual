using System.Net.Sockets;
using System.Text.Json;
using System.Text;

const string host = "127.0.0.1";
const int port = 49123;

using TcpClient client = new TcpClient();
client.Connect(host, port);
