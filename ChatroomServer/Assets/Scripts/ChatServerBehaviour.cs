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

    public string ChatRoomName { get; private set; }

    //private TMP_Text _displayMessages;
    //private List<string> _chatMessagesHistory = new List<string>(); 

    protected override void OnMessage(MessageEventArgs e)
    {
        //Sessions.Broadcast(e.Data);
        string message = e.Data;
        BroadcastMessages(message, this);

        if (e.Data.StartsWith("CreateChatRoom: "))
        {
            string chatRoomName = e.Data.Substring("CreateChatRoom: ".Length);
            chatRooms.Add(chatRoomName);

            BroadcastChatRoomList();
        }
    }

    protected override void OnOpen()
    {
        if (!_clients.Contains(this))
        {
            _clients.Add(this);
            Debug.Log($"Client connected. Total clients: {_clients.Count}");

            ChatRoomName = "";
        }
    }

    protected override void OnClose(CloseEventArgs e)
    {
        _clients.Remove(this);
        Debug.Log($"Client disconnected. Total clients: {_clients.Count}");
    }

    private void BroadcastMessages(string message, ChatServerBehaviour sender)
    {
        Sessions.Broadcast(message);

        foreach (var client in _clients)
        {
            if (client != sender && client.ChatRoomName == sender?.ChatRoomName)
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

    private void JoinChatRoom(string chatRoomName, ChatServerBehaviour client)
    {
        // Set the chat room name for the client
        client.ChatRoomName = chatRoomName;

        // Notify the client that they have joined the chat room
        client.Send($"JoinChatRoom: {chatRoomName}");
    }


}
