using System.Net.Sockets;
using System.Text.Json;
using System.Text;

const string host = "127.0.0.1";
const int port = 49123;

// Create client and connect to port
using TcpClient client = new TcpClient(host, port);
NetworkStream stream = client.GetStream();

// this is where all the data will be
byte[] buffer = new byte[16384];

while (true)
{
    // populate the buffer with data by overriding, items 0 to the amount of data read, of the buffer with new data
    // returns how much data was written
    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
    // if no data then skip
    if (bytesRead == 0) continue;

    // converts bytes to string
    // gets the bytes sat between item 0 and bytesread of the buffer (all the data that was just overriden)
    string incoming = Encoding.UTF8.GetString(buffer, 0, bytesRead);

    try
    {
        // parses everything
        using JsonDocument all = JsonDocument.Parse(incoming);

        // tries to find the data part, sets it to data element
        all.RootElement.TryGetProperty("Data", out JsonElement dataElement);

        // converts the element to a string to re parse
        string? dataElementToParse = dataElement.GetString();
        // if empty then skip
        if (dataElementToParse == null) continue;

        using JsonDocument data = JsonDocument.Parse(dataElementToParse);
        // gets the players part of data
        data.RootElement.TryGetProperty("Players", out JsonElement players);

        // if no players then skip
        if (players.GetArrayLength() == 0) continue;
        // the first player (will have to change to find a certain player)
        JsonElement player = players[0];

        // try and get the property, if below is true then there is no speed in this stream and it should continue
        // otherwise it sets speed element to the speed element
        if (!player.TryGetProperty("Speed", out JsonElement speedElement)) continue;

        // read the speed
        double speed = speedElement.GetDouble();
        Console.WriteLine($"Speed: {speed}");
    }
    catch
    {
        continue;
    }
}
