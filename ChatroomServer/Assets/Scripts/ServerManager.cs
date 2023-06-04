using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp.Server;

public class ServerManager : MonoBehaviour
{
    private WebSocketServer _server;

    public TMP_InputField inputField;

    private void Start()
    {
        _server = new WebSocketServer(System.Net.IPAddress.Parse("127.0.0.1"), 8080);
        _server.AddWebSocketService<ChatServer>("/chat");
        _server.Start();
    }

    private void Update()
    {
        OnApplicationQuit();
    }

    private void OnApplicationQuit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(StopServer());
            Application.Quit();
        }
        
    }

    IEnumerator StopServer()
    {
        yield return new WaitForSecondsRealtime(3);
        _server.Stop();
    }
}
