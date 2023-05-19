using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp.Server;

public class ServerManager : MonoBehaviour
{
    private WebSocketServer _server;

    private void Start()
    {
        _server = new WebSocketServer(System.Net.IPAddress.Parse("127.0.0.1"), 8080);
        _server.AddWebSocketService<ChatServer>("/chat");
        _server.Start();
    }

    private void OnApplicationQuit()
    {
        _server.Stop();
    }
}
