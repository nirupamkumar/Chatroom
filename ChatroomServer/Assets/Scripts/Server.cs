using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp.Server;

public class Server : MonoBehaviour
{
    private WebSocketServer _server;

    public TMP_InputField inputField;

    private void Start()
    {
        try
        {
            _server = new WebSocketServer(15001);
            _server.AddWebSocketService<ChatServerBehaviour>("/chat");
            _server.Start();
            inputField.text = "Server started";
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to start server: {ex.Message}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopServer();
        }
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }

    private void StopServer()
    {
        if (_server != null)
        {
            _server.Stop();
            _server = null;
            Debug.Log("Server stopped");
        }
    }

    public void QuitApplicationWindow()
    {
        Application.Quit();
    }
}