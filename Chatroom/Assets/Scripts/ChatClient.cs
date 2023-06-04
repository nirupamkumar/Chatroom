using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using System;

public class ChatClient : MonoBehaviour
{
    private WebSocket webSocketClient;

    [Header("Identifier")]
    public TMP_InputField identifierInput;

    [Header("Join/Create chatroom")]
    public TMP_InputField createRoomNameInput;
    public TMP_InputField joinRoomNameInput;
    public TMP_Dropdown roomListDropdown;

    [Header("Chartoom")]
    public TMP_InputField chatInputField;

    private void Start()
    {
        webSocketClient = new WebSocket("ws://localhost:15001/chat");
        webSocketClient.OnMessage += OnMessageReceived;
        webSocketClient.Connect();

        webSocketClient.Send(chatInputField.text);
    }

    private void Update()
    {
        QuitApplication();
    }


    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        Console.WriteLine("Connection opened");
        chatInputField.text = e.Data;
    }

    public void SendMessage()
    {
        string playerIdentifier = identifierInput.text;

        string message = "[" + playerIdentifier + "]" + ":" + chatInputField.text;
        webSocketClient.Send(message);
        chatInputField.text = string.Empty;
    }

    public void CreateChatRoom()
    {
        string chatRoomName = joinRoomNameInput.text;
        webSocketClient.Send($"CreateCharRoom: { chatRoomName}");
        chatInputField.text = string.Empty;
    }

    public void JoinChatRoom()
    {
        //join chat room code here
    }

    public void Leavechat()
    {
        webSocketClient.Close();
        //Websocket.Sent(identifierInput + "left the chat");
    }

    public void GetRoomList(List<string> chatRooms)
    {
        roomListDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();

        foreach (string room in chatRooms)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(room);
            optionDatas.Add(option);
        }

        roomListDropdown.AddOptions(optionDatas);
    }

    public void QuitApplication()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
