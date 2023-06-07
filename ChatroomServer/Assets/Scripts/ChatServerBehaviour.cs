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
    private static List<string> chatRooms = new List<string>();

    //private TMP_Text _displayMessages;
    //private List<string> _chatMessagesHistory = new List<string>(); 

    protected override void OnMessage(MessageEventArgs e)
    {
        //Sessions.Broadcast(e.Data);
        string message = e.Data;
        BroadcastMessages(message);

        if (e.Data.StartsWith("CreateChatRoom: "))
        {
            string chatRoomName = e.Data.Substring("CreateChatRoom: ".Length);
            chatRooms.Add(chatRoomName);

            // Broadcast the updated list of chat room names to all clients
            BroadcastChatRoomList();
        }

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
        Sessions.Broadcast(message);

        /*
        _chatMessagesHistory.Add(message);
         Update the displayMessages text with all the stored messages
        _displayMessages.text = string.Join("\n", _chatMessagesHistory) + "\n";

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
        */
    }

    private void BroadcastChatRoomList()
    {
        string chatRoomListMessage = "ChatRoomList: " + string.Join(",", chatRooms.ToArray());
        Sessions.Broadcast(chatRoomListMessage);
    }

}
