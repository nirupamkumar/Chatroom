using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class ChatServer : WebSocketBehavior
{
    private static List<string> _charRooms = new List<string>();

    protected override void OnMessage(MessageEventArgs e)
    {
        //base.OnMessage(e);
        Sessions.Broadcast(e.Data);

        string message = e.Data;

    }

    protected override void OnOpen()
    {
        //base.OnOpen();
    }

    protected override void OnClose(CloseEventArgs e)
    {
        //base.OnClose(e);
    }
}
