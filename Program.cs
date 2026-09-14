using System.Net.Sockets;
using System.Text.Json;
using System.Text;

const string host = "127.0.0.1";
const int port = 49123;

// Create client and connect to port
using TcpClient client = new TcpClient(host, port);
NetworkStream stream = client.GetStream();

// this is where all the data will be
byte[] buffer = new byte[1024];

while (true)
{
    // populate the buffer with data by overriding, items 0 to the amount of data read, of the buffer with new data
    // returns how much data was written
    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

    // converts bytes to string
    // gets the bytes sat between item 0 and bytesread of the buffer (all the data that was just overriden)
    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    Console.WriteLine(data);
}
