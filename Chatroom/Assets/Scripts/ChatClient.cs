using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using System;
using Unity.VisualScripting;

public class ChatClient : MonoBehaviour
{
    public TMP_InputField identifierInput;
    public TMP_InputField chatInputField;
    public TMP_InputField chatRoomNameInput;
    public TMP_Dropdown roomListDropdown;
    public TMP_Text chatText;

    private WebSocket webSocket;

    private void Start()
    {

        webSocket = new WebSocket("ws://127.0.0.1:8080/chat");
        webSocket.OnMessage += OnMessageReceived;
        webSocket.Connect();

        webSocket.Send(chatInputField.text);
    }

    

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        Console.WriteLine("Connection opened");
        chatText.text = e.Data;
    }

    public void SendMessage()
    {
        string playerIdentifier = identifierInput.text;

        string message = "[" + playerIdentifier + "]" + ":" + chatInputField.text;
        webSocket.Send(message);
        chatInputField.text = string.Empty;
    }

    public void CreateChatRoom()
    {
        string chatRoomName = chatRoomNameInput.text;
        webSocket.Send($"CreateCharRoom: { chatRoomName}");
        chatInputField.text = string.Empty;
    }

    public void JoinChatRoom()
    {

    }

    public void Leavechat()
    {
        webSocket.Close();
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

    public void QuitButton()
    {
        Application.Quit();
    }
}
