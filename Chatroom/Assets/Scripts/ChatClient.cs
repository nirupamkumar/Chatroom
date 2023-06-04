using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using System;
using Unity.VisualScripting;

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
    public TMP_Text displayChatText;

    private void Start()
    {
        webSocketClient = new WebSocket("ws://localhost:15001/chat");
        webSocketClient.OnMessage += OnMessageReceived;
        webSocketClient.Connect();

        chatInputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void Update()
    {
        QuitApplication();
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        chatInputField.text = "Connection opened";

        // update chat input field with the received messages
        chatInputField.text = e.Data;

        // add the received messages to the displayChatText
        displayChatText.text += e.Data + "\n";
    }

    private void OnEndEdit(string inputText)
    {
        if (!string.IsNullOrEmpty(inputText) && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    public void SendMessage()
    {
        string playerIdentifier = identifierInput.text;
        string message = "[" + playerIdentifier + "]: " + chatInputField.text;

        // send the message through websockets
        webSocketClient.Send(message);

        // clear the chat input field
        chatInputField.text = string.Empty;

        // Add the sent message to the Textmeshpro Text display
        displayChatText.text += message + "\n";
    }
   
    public void Leavechat()
    {
        webSocketClient.Close();

        string leaveMessage = identifierInput.text + " left the chat";
        webSocketClient.Send(leaveMessage);
    }

    public void CreateChatRoom()
    {
        string chatRoomName = joinRoomNameInput.text;
        webSocketClient.Send($"CreateCharRoom: {chatRoomName}");
        chatInputField.text = string.Empty;
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

    public void JoinChatRoom()
    {
        //join chat room code here
    }

    public void QuitApplication()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
