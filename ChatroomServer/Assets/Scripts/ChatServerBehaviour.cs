using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class ChatServerBehaviour : WebSocketBehavior
{
    private static List<ChatServerBehaviour> _clients = new List<ChatServerBehaviour>();

    //private TMP_Text _displayMessages;
    //private List<string> _chatMessagesHistory = new List<string>(); 

    public ChatServerBehaviour()
    {
        
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        //Sessions.Broadcast(e.Data);
        string message = e.Data;
        BroadcastMessages(message);
    }

    protected override void OnOpen()

    {
        _clients.Add(this);
        Debug.Log($"Client connected. Total clients: {_clients.Count}");
    }

    protected override void OnClose(CloseEventArgs e)
    {
        _clients.Remove(this);
        Debug.Log($"Client disconnected. Total clients: {_clients.Count}");
    }

    private void BroadcastMessages(string message)
    {
        //_chatMessagesHistory.Add(message);
        // Update the displayMessages text with all the stored messages
        //_displayMessages.text = string.Join("\n", _chatMessagesHistory) + "\n";

        // Broadcast the messages to all clients
        foreach (var client in _clients)
        {
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error sending message to client: {ex.Message}");
            }
        }
    }
}
